using SyncronizationBot.Application.UpdateCommand.Base.Handler;
using SyncronizationBot.Application.UpdateCommand.Wallet.Command;
using SyncronizationBot.Application.UpdateCommand.Wallet.Response;
using SyncronizationBot.Domain.Repository.Base.Interfaces;
using SyncronizationBot.Domain.Repository.UnitOfWork;
using Entity = SyncronizationBot.Domain.Model.Database;

namespace SyncronizationBot.Application.UpdateCommand.Wallet.Handler
{
    public class WalletUpdateCommandHandler : BaseUpdateCommandHandler<WalletUpdateCommand, WalletUpdateCommandResponse, Entity.Wallet>
    {
        public WalletUpdateCommandHandler(IUnitOfWorkSqlServer unitOfWorkSqlServer) : base(unitOfWorkSqlServer)
        {
        }
    }
}
