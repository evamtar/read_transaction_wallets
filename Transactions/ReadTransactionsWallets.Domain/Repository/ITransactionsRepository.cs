﻿using ReadTransactionsWallets.Domain.Model.Database;
using ReadTransactionsWallets.Domain.Repository.Base;


namespace ReadTransactionsWallets.Domain.Repository
{
    public interface ITransactionsRepository : IRepository<Transactions>
    {
    }
}
