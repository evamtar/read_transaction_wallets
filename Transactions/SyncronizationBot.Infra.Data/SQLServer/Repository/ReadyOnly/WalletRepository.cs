using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.Base.Interfaces;
using SyncronizationBot.Infra.Data.SQLServer.Context;

namespace SyncronizationBot.Infra.Data.SQLServer.Repository.ReadyOnly
{
    public class WalletRepository : SqlServerReadyOnlyRepository<Wallet>, IWalletRepository
    {
        public WalletRepository(SqlServerReadyOnlyContext context) : base(context)
        {
        }
    }
}
