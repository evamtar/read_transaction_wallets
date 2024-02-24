using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Repository.SQLServerReadyOnly.Base;

namespace SyncronizationBot.Domain.Repository.SQLServer.Base
{
    public interface ISqlServerRepository<T> : ISqlServerReadCommandRepository<T>,
                                                        ISqlServerWriteCommandRepository<T>
                                       where T : Entity
    {

    }
}
