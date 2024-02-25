using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.Base.Interfaces;
using SyncronizationBot.Infra.Data.SQLServer.Context;


namespace SyncronizationBot.Infra.Data.SQLServer.Repository
{
    public class TransactionRPCRecoveryRepository : SqlServerRepository<TransactionRPCRecovery>, ITransactionRPCRecoveryRepository
    {
        public TransactionRPCRecoveryRepository(SqlServerContext context) : base(context)
        {
        }
    }
}
