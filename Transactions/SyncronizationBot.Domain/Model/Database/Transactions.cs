﻿using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Model.Enum;

namespace SyncronizationBot.Domain.Model.Database
{
    public class Transactions: Entity
    {
        public string? Signature { get; set; }
        public DateTime? DateOfTransaction { get; set; }
        public decimal? AmountValueSource { get; set; }
        public decimal? AmountValueSourcePool { get; set; }
        public decimal? AmountValueDestination { get; set;}
        public decimal? AmountValueDestinationPool { get; set; }
        public decimal? FeeTransaction { get; set; }
        public decimal? MtkcapTokenSource { get; set; }
        public decimal? MtkcapTokenSourcePool { get; set; }
        public decimal? MtkcapTokenDestination { get; set; }
        public decimal? MtkcapTokenDestinationPool { get; set; }
        public decimal? PriceTokenSourceUSD { get; set; }
        public decimal? PriceTokenSourcePoolUSD { get; set; }
        public decimal? PriceTokenDestinationUSD { get; set; }
        public decimal? PriceTokenDestinationPoolUSD { get; set; }
        public decimal? PriceSol { get; set; }
        public decimal? TotalTokenSource { get; set; }
        public decimal? TotalTokenSourcePool { get; set; }
        public decimal? TotalTokenDestination { get; set; }
        public decimal? TotalTokenDestinationPool { get; set; }
        public Guid? TokenSourceId { get; set; }
        public Guid? TokenSourcePoolId { get; set; }
        public Guid? TokenDestinationId { get; set; }
        public Guid? TokenDestinationPoolId { get; set; }
        public Guid? WalletId { get; set; }
        public string? WalletHash { get; set; }
        public string? ClassWallet { get; set; }
        public ETypeOperation TypeOperation { get; set; }
        public virtual Token? TokenSource { get; set; }
        public virtual Token? TokenSourcePool { get; set; }
        public virtual Token? TokenDestination { get; set; }
        public virtual Token? TokenDestinationPool { get; set; }
        public virtual Wallet? Wallet { get; set; }
    }
}
