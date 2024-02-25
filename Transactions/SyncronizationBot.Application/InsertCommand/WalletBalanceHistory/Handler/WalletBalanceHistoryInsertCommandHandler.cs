using SyncronizationBot.Application.InsertCommand.Base.Handler;
using SyncronizationBot.Application.InsertCommand.WalletBalanceHistory.Command;
using SyncronizationBot.Application.InsertCommand.WalletBalanceHistory.Response;
using SyncronizationBot.Domain.Repository.UnitOfWork;
using Entity = SyncronizationBot.Domain.Model.Database;

namespace SyncronizationBot.Application.InsertCommand.WalletBalanceHistory.Handler
{
    public class WalletBalanceHistoryInsertCommandHandler : BaseInsertCommandHandler<WalletBalanceHistoryInsertCommand, WalletBalanceHistoryInsertCommandResponse, Entity.WalletBalanceHistory>
    {
        public WalletBalanceHistoryInsertCommandHandler(IUnitOfWorkSqlServer unitOfWorkSqlServer) : base(unitOfWorkSqlServer)
        {
        }
    }
}
