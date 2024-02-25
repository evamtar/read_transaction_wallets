﻿using SyncronizationBot.Application.InsertCommand.Base.Handler;
using SyncronizationBot.Application.InsertCommand.WalletBalance.Command;
using SyncronizationBot.Application.InsertCommand.WalletBalance.Response;
using SyncronizationBot.Domain.Repository.UnitOfWork;
using Entity = SyncronizationBot.Domain.Model.Database;

namespace SyncronizationBot.Application.InsertCommand.WalletBalance.Handler
{
    public class WalletBalanceInsertCommandHandler : BaseInsertCommandHandler<WalletBalanceInsertCommand, WalletBalanceInsertCommandResponse, Entity.WalletBalance>
    {
        public WalletBalanceInsertCommandHandler(IUnitOfWorkSqlServer unitOfWorkSqlServer) : base(unitOfWorkSqlServer)
        {
        }
    }
}
