using SyncronizationBot.Application.InsertCommand.Base.Command;
using SyncronizationBot.Application.InsertCommand.WalletBalance.Response;
using Entity = SyncronizationBot.Domain.Model.Database;


namespace SyncronizationBot.Application.InsertCommand.WalletBalance.Command
{
    public class WalletBalanceInsertCommand : BaseInsertCommand<WalletBalanceInsertCommandResponse, Entity.WalletBalance>
    {
    }
}
