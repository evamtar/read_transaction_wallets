using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.MongoDB.Base;
using SyncronizationBot.Domain.Repository.SQLServer.Base;


namespace SyncronizationBot.Domain.Repository.MongoDB
{
    public interface ITransactionsRepository : IMongoRepository<Transactions>
    {
    }
}
