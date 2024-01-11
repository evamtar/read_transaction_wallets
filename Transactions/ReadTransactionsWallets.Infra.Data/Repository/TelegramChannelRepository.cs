using ReadTransactionsWallets.Domain.Model.Database;
using ReadTransactionsWallets.Domain.Repository;
using ReadTransactionsWallets.Infra.Data.Context;
using ReadTransactionsWallets.Infra.Data.Repository.Base;


namespace ReadTransactionsWallets.Infra.Data.Repository
{
    public class TelegramChannelRepository : Repository<TelegramChannel>, ITelegramChannelRepository
    {
        public TelegramChannelRepository(SqlContext context) : base(context)
        {

        }
    }
}
