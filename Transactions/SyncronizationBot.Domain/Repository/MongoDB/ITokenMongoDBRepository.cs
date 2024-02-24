using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.Base.Interfaces;

namespace SyncronizationBot.Domain.Repository.MongoDB
{
    public interface ITokenMongoDBRepository : IRepository<Token>
    {
    }
}
