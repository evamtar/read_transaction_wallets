using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.SQLServerReadyOnly;
using SyncronizationBot.Infra.Data.SQLServerReadyOnly.Context;
using SyncronizationBot.Infra.Data.SQLServerReadyOnly.Repository.Base;


namespace SyncronizationBot.Infra.Data.SQLServerReadyOnly.Repository
{
    public class TokenAlphaConfigurationReadyOnlyRepository : SqlServerReadyOnlyRepository<TokenAlphaConfiguration>, ITokenAlphaConfigurationReadyOnlyRepository
    {
        public TokenAlphaConfigurationReadyOnlyRepository(SqlServerReadyOnlyContext context) : base(context)
        {

        }
    }
}
