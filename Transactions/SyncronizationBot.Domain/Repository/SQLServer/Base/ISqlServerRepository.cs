using SyncronizationBot.Domain.Model.Database.Base;

namespace SyncronizationBot.Domain.Repository.SQLServer.Base
{
    public interface ISqlServerRepository<T> : ISqlServerReadCommandRepository<T>,
                                               ISqlServerWriteCommandRepository<T>
                                     where T : Entity
    {

    }
}
