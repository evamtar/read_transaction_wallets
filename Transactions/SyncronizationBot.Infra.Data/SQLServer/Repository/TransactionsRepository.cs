using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.Base.Interfaces;
using SyncronizationBot.Infra.Data.SQLServer.Context;

namespace SyncronizationBot.Infra.Data.SQLServer.Repository
{
    public class TransactionsRepository : SqlServerRepository<Transactions>, ITransactionsRepository
    {
        public TransactionsRepository(SqlServerContext context) : base(context)
        {
        }
    }
}
