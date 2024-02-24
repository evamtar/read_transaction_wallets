using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.Base.Interfaces;
using SyncronizationBot.Domain.Repository.Base.Interfaces;


namespace SyncronizationBot.Domain.Repository.MongoDB
{
    public interface ITransactionsMongoDBRepository : IRepository<Transactions>
    {
    }
}
