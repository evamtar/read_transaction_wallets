using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.SQLServerReadyOnly;
using SyncronizationBot.Infra.Data.SQLServerReadyOnly.Context;
using SyncronizationBot.Infra.Data.SQLServerReadyOnly.Repository.Base;


namespace SyncronizationBot.Infra.Data.SQLServerReadyOnly.Repository
{
    public class TypeOperationReadyOnlyRepository : SqlServerReadyOnlyRepository<TypeOperation>, ITypeOperationReadyOnlyRepository
    {
        public TypeOperationReadyOnlyRepository(SqlServerReadyOnlyContext context) : base(context)
        {

        }
    }
}
