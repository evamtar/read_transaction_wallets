using MediatR;
using SyncronizationBot.Application.Commands;
using SyncronizationBot.Application.Commands.MainCommands.RecoverySave;
using SyncronizationBot.Application.Commands.MainCommands.Send;
using SyncronizationBot.Application.Response.MainCommands.RecoverySave;
using SyncronizationBot.Application.Response.MainCommands.Send;
using SyncronizationBot.Domain.Model.CrossCutting.Jupiter.Prices.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Jupiter.Prices.Response;
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
            var alerts = await _alertPriceRepository.Get(x => x.EndDate >= DateTime.Now || x.EndDate == null);
            var prices = await _jupiterPriceService.ExecuteRecoveryPriceAsync(new JupiterPricesRequest { Ids = GetIdsAlerts(alerts.ToList()) });
            foreach (var alert in alerts)
            {
                if (prices!.Data!.ContainsKey(alert.TokenHash!))
                {
                    var price = prices?.Data[alert.TokenHash!];
                    var token = await _mediator.Send(new RecoverySaveTokenCommand { TokenHash = alert.TokenHash });
                    var isSendAlert = false;
                    switch (alert.TypeAlert!)
                    {
                        case ETypeAlertPrice.UP:
                            if (price!.Price >= alert.PriceValue || price!.Price >= alert.PriceValue + alert.PriceBase * alert.PriceBase)
                            {
                                await SendAlertMessage(alert, token, price, EClassifictionMessage.PRICE_UP);
                                isSendAlert = true;
                            }
                            break;
                        case ETypeAlertPrice.DOWN:
                            if (price!.Price <= alert.PriceValue || price!.Price <= alert.PriceBase + alert.PriceBase * alert.PricePercent)
                            {
                                await SendAlertMessage(alert, token, price, EClassifictionMessage.PRICE_DOWN);
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
                            alert.PriceBase = price?.Price;
                            await _alertPriceRepository.Edit(alert);
                            await _alertPriceRepository.DetachedItem(alert);
                        }
                        else 
                        {
                            alert.EndDate = DateTime.Now;
                            await _alertPriceRepository.Edit(alert);
                            await _alertPriceRepository.DetachedItem(alert);
                        }
                    }
                }
            }
            return new SendAlertPriceCommandResponse();
        }

        private async Task SendAlertMessage(AlertPrice alert, RecoverySaveTokenCommandResponse token, TokenData price, EClassifictionMessage type)
        {
            await this._mediator.Send(new SendAlertMessageCommand
            {
                Parameters = SendAlertMessageCommand.GetParameters(new object[] { alert, token, price }),
                TypeAlert = ETypeAlert.ALERT_PRICE,
                IdClassification = (int?)type
            });
        }

        private List<string> GetIdsAlerts(List<AlertPrice> alerts)
        {
            var listIdsTokens = new List<string>();
            foreach (var alert in alerts)
            {
                if (alert.TokenHash != null)
                {
                    if (!listIdsTokens.Contains(alert.TokenHash))
                        listIdsTokens.Add(alert.TokenHash);
                }
            }
            return listIdsTokens;
        }
    }

    public enum EClassifictionMessage 
    { 
        PRICE_UP = 1,
        PRICE_DOWN = 2
    }
}
