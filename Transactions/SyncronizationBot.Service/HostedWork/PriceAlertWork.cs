using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.ExternalServiceCommand.ExternalServiceRead.MultExternal.Price.Command;
using SyncronizationBot.Application.Handlers.MainCommands.Send;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Model.RabbitMQ;
using SyncronizationBot.Domain.Service.HostedWork;
using SyncronizationBot.Domain.Service.HostedWork.Base;
using SyncronizationBot.Domain.Service.InternalService.Domains;
using SyncronizationBot.Domain.Service.InternalService.Price;
using SyncronizationBot.Domain.Service.RabbitMQ.Queue.AlertPriceQueue;
using SyncronizationBot.Domain.Service.RabbitMQ.Queue.UpdateQueue;
using SyncronizationBot.Utils;

namespace SyncronizationBot.Service.HostedWork
{
    public class PriceAlertWork : IPriceAlertWork
    {
        private readonly IMediator _mediator;
        private readonly IAlertPriceService _alertPriceService;
        private readonly ITypeOperationService _typeOperationService;
        private readonly IPublishAlertPriceService _publishAlertPriceService;
        private readonly IPublishUpdateService _publishUpdateService;
        public IOptions<SyncronizationBotConfig>? Options => throw new NotImplementedException();
        public ETypeService? TypeService => ETypeService.PriceAlert;

        public PriceAlertWork(IMediator mediator, 
                              IAlertPriceService alertPriceService,
                              ITypeOperationService typeOperationService,
                              IPublishAlertPriceService publishAlertPriceService,
                              IPublishUpdateService publishUpdateService)
        {
            this._mediator = mediator;
            this._alertPriceService = alertPriceService;
            this._typeOperationService = typeOperationService;
            this._publishAlertPriceService = publishAlertPriceService;
            this._publishUpdateService = publishUpdateService;
        }

        public async Task DoExecute(CancellationToken cancellationToken)
        {
            var typeOperation = await this._typeOperationService.FindFirstOrDefaultAsync(x => x.IdTypeOperation == 0  && x.IdSubLevel == (int)ESubLeveTypeOperation.AlertPrice);
            var alerts = await this._alertPriceService.GetAsync(x => (x.EndDate >= DateTime.Now || x.EndDate == null));
            if (alerts?.Count > 0) 
            {
                foreach (var alert in alerts)
                {
                    var token = await this._mediator.Send(new ReadTokenPriceCommand { TokenHash = alert.TokenHash! });
                    var isSendAlert = false;
                    
                    switch (alert.TypeAlert!)
                    {
                        case ETypeAlertPrice.UP:
                            if (token.PriceUsd >= alert.PriceValue || token.PriceUsd >= alert.PriceValue + alert.PriceBase * alert.PriceBase)
                            {
                                var @event = new MessageEvent<AlertPrice> 
                                {
                                    EventName  = typeof(AlertPrice).Name,
                                    CreateDate = DateTime.Now,
                                    Entity = alert,
                                    Parameters = Converters.GetParameters(new object[] { alert, token }),
                                    TypeOperationId = typeOperation?.ID,
                                    IdSubLevelAlertConfiguration = (int)EClassifictionMessage.PRICE_UP
                                };
                                await this._publishAlertPriceService.Publish(@event);
                                isSendAlert = true;
                            }
                            break;
                        case ETypeAlertPrice.DOWN:
                            if (token.PriceUsd <= alert.PriceValue || token.PriceUsd <= alert.PriceBase + alert.PriceBase * alert.PricePercent)
                            {
                                var @event = new MessageEvent<AlertPrice>
                                {
                                    EventName = typeof(AlertPrice).Name,
                                    CreateDate = DateTime.Now,
                                    Entity = alert,
                                    Parameters = Converters.GetParameters(new object[] { alert, token }),
                                    TypeOperationId = typeOperation?.ID,
                                    IdSubLevelAlertConfiguration = (int)EClassifictionMessage.PRICE_DOWN
                                };
                                await this._publishAlertPriceService.Publish(@event);
                                isSendAlert = true;
                            }
                            break;
                        default:
                            break;
                    }
                    if (isSendAlert)
                    {
                        if (alert.IsRecurrence ?? false)
                        {
                            if (alert.PricePercent == null)
                                alert.PriceValue += alert.PriceValue - alert.PriceBase;
                            alert.PriceBase = token?.PriceUsd;
                        }
                        else
                            alert.EndDate = DateTime.Now;
                        this._alertPriceService.Update(alert);
                        await this._publishUpdateService.Publish(new MessageEvent<AlertPrice>
                        {
                            EventName = typeof(AlertPrice).Name + "_" + Constants.INSTRUCTION_UPDATE,
                            CreateDate = DateTime.Now,
                            Entity = alert,
                            Parameters = Converters.GetParameters(new object[] { alert, token }),
                            TypeOperationId = typeOperation?.ID,
                            IdSubLevelAlertConfiguration = (int)EClassifictionMessage.PRICE_DOWN
                        });
                    }
                }
            }
        }

        public void Dispose()
        {
            try
            {
                this._alertPriceService.Dispose();
                this._typeOperationService.Dispose();
            }
            finally 
            { 
                GC.SuppressFinalize(this);
            }
        }
    }
}
