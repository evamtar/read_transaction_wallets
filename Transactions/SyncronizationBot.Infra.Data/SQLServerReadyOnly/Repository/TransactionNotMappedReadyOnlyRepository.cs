using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.SQLServerReadyOnly;
using SyncronizationBot.Infra.Data.SQLServerReadyOnly.Context;
using SyncronizationBot.Infra.Data.SQLServerReadyOnly.Repository.Base;

namespace SyncronizationBot.Infra.Data.SQLServerReadyOnly.Repository
{
    public class TransactionNotMappedReadyOnlyRepository : SqlServerReadyOnlyRepository<TransactionNotMapped>, ITransactionNotMappedReadyOnlyRepository
    {
        public TransactionNotMappedReadyOnlyRepository(SqlServerReadyOnlyContext context) : base(context)
        {

        }
    }
}
