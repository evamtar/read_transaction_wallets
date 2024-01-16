﻿using SyncronizationBot.Domain.Model.Database.Base;


namespace SyncronizationBot.Domain.Model.Database
{
    public class Wallet : Entity
    {
        public string? Hash { get; set; }
        public Guid? IdClassWallet { get; set; }
        public bool? IsLoadBalance { get; set; }
        public bool? IsActive { get; set; }
        public virtual ClassWallet? ClassWallet { get; set; }
        public virtual List<Transactions>? Transactions { get; set; }
        public virtual List<WalletBalance>? Balances { get; set; }
    }
}