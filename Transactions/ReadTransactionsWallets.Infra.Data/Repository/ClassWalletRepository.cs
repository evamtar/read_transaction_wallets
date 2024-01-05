using ReadTransactionsWallets.Domain.Model.Database;
using ReadTransactionsWallets.Domain.Repository;
using ReadTransactionsWallets.Infra.Data.Context;
using ReadTransactionsWallets.Infra.Data.Repository.Base;


namespace ReadTransactionsWallets.Infra.Data.Repository
{
    public class ClassWalletRepository : Repository<ClassWallet>, IClassWalletRepository
    {
        public ClassWalletRepository(SqlContext context) : base(context)
        {

        }
    }
}
