using Entity = SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Application.UpdateCommand.Base.Command;
using SyncronizationBot.Application.UpdateCommand.Wallet.Response;

namespace SyncronizationBot.Application.UpdateCommand.Wallet.Command
{
    public class WalletUpdateCommand : BaseUpdateCommand<WalletUpdateCommandResponse, Entity.Wallet>
    {
    }
}
