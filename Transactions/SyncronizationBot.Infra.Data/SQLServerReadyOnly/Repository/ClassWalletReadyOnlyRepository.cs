using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.SQLServerReadyOnly;
using SyncronizationBot.Infra.Data.SQLServerReadyOnly.Context;
using SyncronizationBot.Infra.Data.SQLServerReadyOnly.Repository.Base;


namespace SyncronizationBot.Infra.Data.SQLServerReadyOnly.Repository
{
    public class ClassWalletReadyOnlyRepository : SqlServerReadyOnlyRepository<ClassWallet>, IClassWalletReadyOnlyRepository
    {
        public ClassWalletReadyOnlyRepository(SqlServerReadyOnlyContext context) : base(context)
        {

        }
    }
}
