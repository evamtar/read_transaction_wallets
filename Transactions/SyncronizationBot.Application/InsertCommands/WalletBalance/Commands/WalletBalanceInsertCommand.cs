using SyncronizationBot.Application.InsertCommands.Base.Commands;
using SyncronizationBot.Application.InsertCommands.WalletBalance.Response;
using Model = SyncronizationBot.Domain.Model.Database;


namespace SyncronizationBot.Application.InsertCommands.WalletBalance.Commands
{
    public class WalletBalanceInsertCommand : BaseInsertCommand<Model.WalletBalance, WalletBalanceInsertCommandResponse>
    {
    }
}
