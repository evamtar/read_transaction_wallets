using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.Base;
using System.Linq.Expressions;

namespace SyncronizationBot.Domain.Repository
{
    public interface ITransactionsRPCRecoveryRepository : IRepository<TransactionsRPCRecovery>
    {
        decimal? GetMaxBlockTime(Expression<Func<TransactionsRPCRecovery, bool>> predicate);
    }
}
