﻿using MediatR;
using SyncronizationBot.Application.Commands;
using SyncronizationBot.Application.Response;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository;
using System.Diagnostics;

namespace SyncronizationBot.Application.Handlers
{
    public class RecoveryAddUpdateBalanceItemCommandHandler : IRequestHandler<RecoveryAddUpdateBalanceItemCommand, RecoveryAddUpdateBalanceItemCommandResponse>
    {
        private readonly IMediator _mediator;
        private readonly IWalletBalanceRepository _walletBalanceRepository;
        private readonly IWalletBalanceHistoryRepository _walletBalanceHistoryRepository;
        public RecoveryAddUpdateBalanceItemCommandHandler(IMediator mediator,
                                                          IWalletBalanceRepository walletBalanceRepository,
                                                          IWalletBalanceHistoryRepository walletBalanceHistoryRepository)
        {
            this._mediator = mediator;
            this._walletBalanceRepository = walletBalanceRepository;
            this._walletBalanceHistoryRepository = walletBalanceHistoryRepository;
        }

        public async Task<RecoveryAddUpdateBalanceItemCommandResponse> Handle(RecoveryAddUpdateBalanceItemCommand request, CancellationToken cancellationToken)
        {
            var balance = await this._walletBalanceRepository.FindFirstOrDefault( x => x.IdToken == request.TokenId && x.IdWallet == request.WalleId );
            var percentage = (decimal?)100;
            var oldQuantity = (decimal?)0;
            if (balance == null)
            {
                var price = await this._mediator.Send(new RecoveryPriceCommand { Ids = new List<string> { request.TokenHash! } });
                balance = await this._walletBalanceRepository.Add(new WalletBalance
                {
                    IdWallet = request.WalleId,
                    IdToken = request.TokenId,
                    TokenHash = request.TokenHash,
                    Quantity = request.Quantity,
                    Price = price?.Data?[request.TokenHash!].Price ?? 0,
                    TotalValueUSD = request.Quantity * price?.Data?[request.TokenHash!].Price,
                    IsActive = request.Quantity > 0,
                    LastUpdate = DateTime.Now
                });
            }
            else 
            {
                oldQuantity = balance.Quantity;
                percentage = this.CalculatePercentege(request.Quantity, balance.Quantity);
                balance.Quantity += request.Quantity;
                balance.IsActive = balance.Quantity > 0;
                if (balance.IsActive ?? false) 
                {
                    var price = await this._mediator.Send(new RecoveryPriceCommand { Ids = new List<string> { request.TokenHash! } });
                    balance.Price = price?.Data?[request.TokenHash!].Price ?? 0;
                    balance.TotalValueUSD = balance.Quantity * price?.Data?[request.TokenHash!].Price;
                }
                balance.LastUpdate = DateTime.Now;
                balance = await this._walletBalanceRepository.Edit(balance);
                try { await this._walletBalanceRepository.DetachedItem(balance); } catch { }
            }
            
            await this._walletBalanceHistoryRepository.Add(new WalletBalanceHistory 
            { 
                IdWalletBalance = balance.ID,
                IdWallet = balance.IdWallet,
                IdToken = balance.IdToken,
                TokenHash = balance.TokenHash,
                OldQuantity = oldQuantity,
                NewQuantity = balance.Quantity,
                RequestQuantity = request.Quantity,
                PercentageCalculated = percentage,
                Price = balance.Price,
                TotalValueUSD = balance.TotalValueUSD,
                Signature = request.Signature,
                CreateDate = DateTime.Now,
                LastUpdate = balance.LastUpdate
            });
            return new RecoveryAddUpdateBalanceItemCommandResponse 
            {
                Quantity = balance.Quantity,
                Price = balance.Price,
                PercentModify = percentage,
                IsActive = balance.IsActive
            };
        }

        private decimal? CalculatePercentege(decimal? quantityEnter, decimal? quantity)
        {
            if (quantity + quantityEnter <= 0)
                return -100;
            else
            {
                var percentage = (quantityEnter / quantity) * 100;
                if (percentage != null)
                    return Math.Round(percentage.Value, 5);
                return percentage;
            }
        }
    }
}
