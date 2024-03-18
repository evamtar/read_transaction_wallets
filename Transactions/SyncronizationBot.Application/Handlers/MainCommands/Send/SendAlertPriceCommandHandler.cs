using MediatR;
using SyncronizationBot.Application.Commands.MainCommands.RecoverySave;
using SyncronizationBot.Application.Commands.MainCommands.Send;
using SyncronizationBot.Application.Response.MainCommands.RecoverySave;
using SyncronizationBot.Application.Response.MainCommands.Send;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Repository;
using SyncronizationBot.Domain.Service.CrossCutting.Jupiter;


namespace SyncronizationBot.Application.Handlers.MainCommands.Send
{
    public class SendAlertPriceCommandHandler : IRequestHandler<SendAlertPriceCommand, SendAlertPriceCommandResponse>
    {
        private readonly IMediator _mediator;
        private readonly IAlertPriceRepository _alertPriceRepository;
        private readonly IJupiterPriceService _jupiterPriceService;

        public SendAlertPriceCommandHandler(IMediator mediator,
                                              IAlertPriceRepository alertPriceRepository,
                                              IJupiterPriceService jupiterPriceService)
        {
            _mediator = mediator;
            _alertPriceRepository = alertPriceRepository;
            _jupiterPriceService = jupiterPriceService;
        }

        public async Task<SendAlertPriceCommandResponse> Handle(SendAlertPriceCommand request, CancellationToken cancellationToken)
        {
            var alerts = await this._alertPriceRepository.Get(x => (x.EndDate >= DateTime.Now || x.EndDate == null));
            if (alerts?.Count > 0)
            {
                foreach (var alert in alerts)
                {
                    var token = await this._mediator.Send(new RecoverySaveTokenCommand { TokenHash = alert.TokenHash });
                    var isSendAlert = false;
                    switch (alert.TypeAlert!)
                    {
                        case ETypeAlertPrice.UP:
                            if (token.Price >= alert.PriceValue || token.Price >= alert.PriceValue + alert.PriceBase * alert.PriceBase)
                            {
                                await SendAlertMessage(alert, token, EClassifictionMessage.PRICE_UP);
                                isSendAlert = true;
                            }
                            break;
                        case ETypeAlertPrice.DOWN:
                            if (token.Price <= alert.PriceValue || token.Price <= alert.PriceBase + alert.PriceBase * alert.PricePercent)
                            {
                                await SendAlertMessage(alert, token, EClassifictionMessage.PRICE_DOWN);
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
                            alert.PriceBase = token?.Price;
                            this._alertPriceRepository.Edit(alert);
                            await this._alertPriceRepository.DetachedItem(alert);
                        }
                        else
                        {
                            alert.EndDate = DateTime.Now;
                            this._alertPriceRepository.Edit(alert);
                            await this._alertPriceRepository.DetachedItem(alert);
                        }
                    }
                }
            }
            return new SendAlertPriceCommandResponse();
        }

        private async Task SendAlertMessage(AlertPrice alert, RecoverySaveTokenCommandResponse token, EClassifictionMessage type)
        {
            await this._mediator.Send(new SendAlertMessageCommand
            {
                Parameters = SendAlertMessageCommand.GetParameters(new object[] { alert, token }),
                TypeAlert = ETypeAlert.ALERT_PRICE,
                IdClassification = (int?)type
            });
        }

        
    }

    public enum EClassifictionMessage 
    { 
        PRICE_UP = 1,
        PRICE_DOWN = 2
    }
}
