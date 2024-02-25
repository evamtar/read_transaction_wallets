
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.Base.Interfaces;
using SyncronizationBot.Infra.Data.SQLServer.Context;

namespace SyncronizationBot.Infra.Data.SQLServer.Repository.ReadyOnly
{
    public class TransactionTokenRepository : SqlServerReadyOnlyRepository<TransactionToken>, ITransactionTokenRepository
    {
        public TransactionTokenRepository(SqlServerReadyOnlyContext context) : base(context)
        {
        }
    }
}
