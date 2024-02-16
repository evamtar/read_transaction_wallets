using SyncronizationBot.Application.InsertCommands.Base.Handlers;
using SyncronizationBot.Application.InsertCommands.WalletBalance.Commands;
using SyncronizationBot.Application.InsertCommands.WalletBalance.Response;
using SyncronizationBot.Domain.Repository;
using Entity = SyncronizationBot.Domain.Model.Database;

namespace SyncronizationBot.Application.InsertCommands.WalletBalance.Handlers
{
    public class WalletBalanceInsertCommandHandler : BaseInsertCommandHandler<WalletBalanceInsertCommand, WalletBalanceInsertCommandResponse, Entity.WalletBalance>
    {
        public WalletBalanceInsertCommandHandler(IWalletBalanceRepository repository) : base(repository)
        {
        }
    }
}
