﻿using MediatR;
using SyncronizationBot.Application.Commands;
using SyncronizationBot.Application.Response;
using SyncronizationBot.Domain.Model.CrossCutting.Jupiter.Prices.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Jupiter.Prices.Response;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Repository;
using SyncronizationBot.Domain.Service.CrossCutting.Jupiter;
using SyncronizationBot.Utils;

namespace SyncronizationBot.Application.Handlers
{
    public class SendAlertMessageCommandHandler : IRequestHandler<SendAlertMessageCommand, SendAlertMessageCommandResponse>
    {
        private readonly IMediator _mediator;
        private readonly IAlertPriceRepository _alertPriceRepository;
        private readonly IJupiterPriceService _jupiterPriceService;

        public SendAlertMessageCommandHandler(IMediator mediator,
                                              IAlertPriceRepository alertPriceRepository,
                                              IJupiterPriceService jupiterPriceService)
        {
            this._mediator = mediator;
            this._alertPriceRepository = alertPriceRepository;
            this._jupiterPriceService = jupiterPriceService;
        }

        public async Task<SendAlertMessageCommandResponse> Handle(SendAlertMessageCommand request, CancellationToken cancellationToken)
        {
            var alerts = await this._alertPriceRepository.Get(x => x.EndDate >= DateTime.Now || x.EndDate == null);
            var prices = await this._jupiterPriceService.ExecuteRecoveryPriceAsync(new JupiterPricesRequest { Ids = this.GetIdsAlerts(alerts.ToList()) });
            foreach (var alert in alerts)
            {
                if (prices!.Data!.ContainsKey(alert.TokenHash!))
                {
                    var price = prices?.Data[alert.TokenHash!];
                    var token = await this._mediator.Send(new RecoverySaveTokenCommand { TokenHash = alert.TokenHash });
                    var isSendAlert = false;
                    switch (alert.TypeAlert!)
                    {
                        case ETypeAlert.UP:
                            if (price!.Price >= alert.PriceValue || (price!.Price >= (alert.PriceValue + (alert.PriceBase * alert.PriceBase))))
                            {
                                await this.SendAlertMessage(alert, token, price, ETypeMessage.PRICE_UP_MESSAGE);
                                isSendAlert = true;
                            }
                            break;
                        case ETypeAlert.DOWN:
                            if (price!.Price <= alert.PriceValue || (price!.Price <= (alert.PriceBase + (alert.PriceBase * alert.PricePercent))))
                            {
                                await this.SendAlertMessage(alert, token, price, ETypeMessage.PRICE_DOWN_MESSAGE);
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
                                alert.PriceValue += (alert.PriceValue - alert.PriceBase);
                            alert.PriceBase = price?.Price;
                            await this._alertPriceRepository.Edit(alert);
                        }
                    }
                }
            }
            return new SendAlertMessageCommandResponse();
        }

        private async Task SendAlertMessage(AlertPrice alert, RecoverySaveTokenCommandResponse token, TokenData price, ETypeMessage type) 
        {
            await this._mediator.Send(new SendTelegramMessageCommand
            {
                Channel = ETelegramChannel.AlertPriceChange,
                Message = TelegramMessageHelper.GetFormatedMessage(type,
                                    new object[] {
                                        alert.TokenHash ?? string.Empty,
                                        token.TokenAlias ?? string.Empty,
                                        price.Price ?? 0,
                                        (alert.IsRecurrence ?? false) ? "YES" : "NO",
                                    })
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
}