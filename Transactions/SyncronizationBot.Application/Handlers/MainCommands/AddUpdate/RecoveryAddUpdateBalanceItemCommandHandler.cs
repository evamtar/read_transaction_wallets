﻿using MediatR;
using SyncronizationBot.Application.Commands.MainCommands.AddUpdate;
using SyncronizationBot.Application.Commands.MainCommands.RecoverySave;
using SyncronizationBot.Application.Response.MainCommands.AddUpdate;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Repository;
using System.Diagnostics;
using System.Transactions;

namespace SyncronizationBot.Application.Handlers.MainCommands.AddUpdate
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
            _mediator = mediator;
            _walletBalanceRepository = walletBalanceRepository;
            _walletBalanceHistoryRepository = walletBalanceHistoryRepository;
        }

        public async Task<RecoveryAddUpdateBalanceItemCommandResponse> Handle(RecoveryAddUpdateBalanceItemCommand request, CancellationToken cancellationToken)
        {
            if (request?.Transactions?.FeeTransaction.HasValue ?? false)
            {
                var solTokenForFee = await _mediator.Send(new RecoverySaveTokenCommand { TokenHash = "So11111111111111111111111111111111111111112" });
                await this.UpdateBalance(request?.Transactions?.WalletId, solTokenForFee?.TokenId, request?.Transactions.Signature, solTokenForFee?.Hash, request?.Transactions?.FeeTransaction);
            }
            switch (request?.Transactions?.TypeOperation)
            {
                case ETypeOperation.BUY:
                case ETypeOperation.SWAP:
                    await this.UpdateBalance(request?.Transactions?.WalletId, request?.Transactions?.TokenSourceId, request?.Transactions?.Signature, request?.TokenSendedHash, request?.Transactions?.AmountValueSource);
                    return await this.UpdateBalance(request?.Transactions?.WalletId, request?.Transactions?.TokenDestinationId, request?.Transactions?.Signature, request?.TokenReceivedHash, request?.Transactions?.AmountValueDestination);
                case ETypeOperation.SELL:
                    await this.UpdateBalance(request?.Transactions?.WalletId, request?.Transactions?.TokenDestinationId, request?.Transactions?.Signature, request?.TokenReceivedHash, request?.Transactions?.AmountValueDestination);
                    return await this.UpdateBalance(request?.Transactions?.WalletId, request?.Transactions?.TokenSourceId, request?.Transactions?.Signature, request?.TokenSendedHash, request?.Transactions?.AmountValueSource);
                case ETypeOperation.SEND:
                    await this.UpdateBalance(request?.Transactions?.WalletId, request?.Transactions?.TokenSourceId, request?.Transactions?.Signature, request?.TokenSendedHash, request?.Transactions?.AmountValueSource);
                    break;
                case ETypeOperation.RECEIVED:
                    await this.UpdateBalance(request?.Transactions?.WalletId, request?.Transactions?.TokenDestinationId, request?.Transactions?.Signature, request?.TokenReceivedHash, request?.Transactions?.AmountValueDestination);
                    break;
                case ETypeOperation.POOLCREATE:
                    await this.UpdateBalance(request?.Transactions?.WalletId, request?.Transactions?.TokenSourceId, request?.Transactions?.Signature, request?.TokenSendedHash, request?.Transactions?.AmountValueSource);
                    await this.UpdateBalance(request?.Transactions?.WalletId, request?.Transactions?.TokenSourcePoolId, request?.Transactions?.Signature, request?.TokenSendedPoolHash, request?.Transactions?.AmountValueSourcePool);
                    break;
                case ETypeOperation.POOLFINALIZED:
                    await this.UpdateBalance(request?.Transactions?.WalletId, request?.Transactions?.TokenDestinationId, request?.Transactions?.Signature, request?.TokenReceivedHash, request?.Transactions?.AmountValueDestination);
                    await this.UpdateBalance(request?.Transactions?.WalletId, request?.Transactions?.TokenDestinationPoolId, request?.Transactions?.Signature, request?.TokenReceivedPoolHash, request?.Transactions?.AmountValueDestinationPool);
                    break;
                case ETypeOperation.NONE:
                case ETypeOperation.BURN:
                    break;
                default:
                    break;
            }
            return null!;
        }

        private async Task<RecoveryAddUpdateBalanceItemCommandResponse> UpdateBalance(Guid? walleId, Guid? tokenId, string? signature, string? tokenHash, decimal? quantity) 
        {
            var balance = await _walletBalanceRepository.FindFirstOrDefault(x => x.TokenId == tokenId && x.WalletId == walleId);
            var percentage = (decimal?)100;
            var oldQuantity = (decimal?)0;
            if (balance == null)
            {
                var price = await _mediator.Send(new RecoveryPriceCommand { Ids = new List<string> { tokenHash! } });
                balance = await this._walletBalanceRepository.Add(new WalletBalance
                {
                    WalletId = walleId,
                    TokenId = tokenId,
                    TokenHash = tokenHash,
                    Quantity = quantity,
                    Price = price?.Data?[tokenHash!].Price ?? 0,
                    TotalValueUSD = quantity * price?.Data?[tokenHash!].Price,
                    IsActive = quantity > 0,
                    LastUpdate = DateTime.Now
                });
                await this._walletBalanceRepository.DetachedItem(balance);
            }
            else
            {
                oldQuantity = balance.Quantity;
                percentage = CalculatePercentege(quantity, balance.Quantity);
                balance.Quantity += quantity;
                balance.IsActive = balance.Quantity > 0;
                if (balance.IsActive ?? false)
                {
                    var price = await _mediator.Send(new RecoveryPriceCommand { Ids = new List<string> { tokenHash! } });
                    balance.Price = price?.Data?[tokenHash!].Price ?? 0;
                    balance.TotalValueUSD = balance.Quantity * price?.Data?[tokenHash!].Price;
                }
                balance.LastUpdate = DateTime.Now;
                balance = await this._walletBalanceRepository.Edit(balance);
                await this._walletBalanceRepository.DetachedItem(balance);
            }

            var walletBalanceHistory = await this._walletBalanceHistoryRepository.Add(new WalletBalanceHistory
            {
                WalletBalanceId = balance.ID,
                WalletId = balance.WalletId,
                TokenId = balance.TokenId,
                TokenHash = balance.TokenHash,
                OldQuantity = oldQuantity,
                NewQuantity = balance.Quantity,
                RequestQuantity = quantity,
                PercentageCalculated = percentage,
                Price = balance.Price,
                TotalValueUSD = balance.TotalValueUSD,
                Signature = signature,
                CreateDate = DateTime.Now,
                LastUpdate = balance.LastUpdate
            });
            await this._walletBalanceHistoryRepository.DetachedItem(walletBalanceHistory);
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
                var percentage = quantityEnter / quantity * 100;
                if (percentage != null)
                    return Math.Round(percentage.Value, 5);
                return percentage;
            }
        }
    }
}
