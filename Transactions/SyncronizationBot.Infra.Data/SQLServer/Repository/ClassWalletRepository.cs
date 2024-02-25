using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.Base.Interfaces;
using SyncronizationBot.Infra.Data.SQLServer.Context;

namespace SyncronizationBot.Infra.Data.SQLServer.Repository
{
    public class ClassWalletRepository : SqlServerRepository<ClassWallet>, IClassWalletRepository
    {
        public ClassWalletRepository(SqlServerContext context) : base(context)
        {
        }
    }
}
