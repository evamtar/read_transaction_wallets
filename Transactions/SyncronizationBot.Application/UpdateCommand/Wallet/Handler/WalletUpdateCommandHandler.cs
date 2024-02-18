using SyncronizationBot.Application.UpdateCommand.Base.Handler;
using SyncronizationBot.Application.UpdateCommand.Wallet.Command;
using SyncronizationBot.Application.UpdateCommand.Wallet.Response;
using SyncronizationBot.Domain.Repository.SQLServer;
using Entity = SyncronizationBot.Domain.Model.Database;

namespace SyncronizationBot.Application.UpdateCommand.Wallet.Handler
{
    public class WalletUpdateCommandHandler : BaseUpdateCommandHandler<WalletUpdateCommand, WalletUpdateCommandResponse, Entity.Wallet>
    {
        public WalletUpdateCommandHandler(IWalletRepository repository) : base(repository)
        {
        }
    }
}
