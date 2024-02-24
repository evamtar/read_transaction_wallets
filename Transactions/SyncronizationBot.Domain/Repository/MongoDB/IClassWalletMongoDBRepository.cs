using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.MongoDB.Base;

namespace SyncronizationBot.Domain.Repository.MongoDB
{
    public interface IClassWalletMongoDBRepository : IMongoRepository<ClassWallet>
    {
    }
}
