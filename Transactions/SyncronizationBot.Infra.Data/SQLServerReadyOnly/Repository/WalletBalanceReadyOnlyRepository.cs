using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.SQLServerReadyOnly;
using SyncronizationBot.Infra.Data.SQLServerReadyOnly.Context;
using SyncronizationBot.Infra.Data.SQLServerReadyOnly.Repository.Base;

namespace SyncronizationBot.Infra.Data.SQLServerReadyOnly.Repository
{
    public class WalletBalanceReadyOnlyRepository : SqlServerReadyOnlyRepository<WalletBalance>, IWalletBalanceReadyOnlyRepository
    {
        public WalletBalanceReadyOnlyRepository(SqlServerReadyOnlyContext context) : base(context)
        {

        }
    }
}
