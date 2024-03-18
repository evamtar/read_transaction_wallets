using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository;
using SyncronizationBot.Infra.Data.Context;
using SyncronizationBot.Infra.Data.Repository.Base;
using System.Linq.Expressions;

namespace SyncronizationBot.Infra.Data.Repository
{
    public class TransactionsRPCRecoveryRepository : Repository<TransactionsRPCRecovery>, ITransactionsRPCRecoveryRepository
    {
        private SqlContext _sqlContext;
        public TransactionsRPCRecoveryRepository(SqlContext context) : base(context)
        {
            _sqlContext = context;
        }

        public decimal? GetMaxBlockTime(Expression<Func<TransactionsRPCRecovery, bool>> predicate)
        {
            return _sqlContext.TransactionsContingencies.Where(predicate).Max(x => x.BlockTime);
        }
    }
}
