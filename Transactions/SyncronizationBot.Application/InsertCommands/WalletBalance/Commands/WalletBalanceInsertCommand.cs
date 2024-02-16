using SyncronizationBot.Application.InsertCommands.Base.Commands;
using SyncronizationBot.Application.InsertCommands.WalletBalance.Response;
using Entity = SyncronizationBot.Domain.Model.Database;


namespace SyncronizationBot.Application.InsertCommands.WalletBalance.Commands
{
    public class WalletBalanceInsertCommand : BaseInsertCommand<WalletBalanceInsertCommandResponse, Entity.WalletBalance>
    {
    }
}
