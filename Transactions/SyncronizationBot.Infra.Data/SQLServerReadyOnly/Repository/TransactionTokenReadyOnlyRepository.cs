using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.SQLServerReadyOnly;
using SyncronizationBot.Infra.Data.SQLServerReadyOnly.Context;
using SyncronizationBot.Infra.Data.SQLServerReadyOnly.Repository.Base;

namespace SyncronizationBot.Infra.Data.SQLServerReadyOnly.Repository
{
    public class TransactionTokenReadyOnlyRepository : SqlServerReadyOnlyRepository<TransactionToken>, ITransactionTokenReadyOnlyRepository
    {
        public TransactionTokenReadyOnlyRepository(SqlServerReadyOnlyContext context) : base(context)
        {

        }
    }
}
