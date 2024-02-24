using SyncronizationBot.Domain.Model.Database.Base;

namespace SyncronizationBot.Domain.Repository.SQLServerReadyOnly.Base
{
    public interface ISqlServerReadyOnlyRepository<T> : ISqlServerReadCommandRepository<T> where T : Entity
    {
    }
}
