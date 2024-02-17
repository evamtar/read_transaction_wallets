using SyncronizationBot.Application.InsertCommand.Base.Command;
using SyncronizationBot.Application.InsertCommand.WalletBalanceHistory.Response;
using Entity = SyncronizationBot.Domain.Model.Database;

namespace SyncronizationBot.Application.InsertCommand.WalletBalanceHistory.Command
{
    public class WalletBalanceHistoryInsertCommand : BaseInsertCommand<WalletBalanceHistoryInsertCommandResponse, Entity.WalletBalanceHistory>
    {
    }
}
