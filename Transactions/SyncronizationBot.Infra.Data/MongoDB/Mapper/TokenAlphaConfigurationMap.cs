using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Infra.Data.Base.Mapper;


namespace SyncronizationBot.Infra.Data.MongoDB.Mapper
{
    public class TokenAlphaConfigurationMap : BaseMapper<TokenAlphaConfiguration>
    {
        public TokenAlphaConfigurationMap() : base(EDatabase.Mongodb)
        {
        }

        protected override void IgnoreProperties(EntityTypeBuilder<TokenAlphaConfiguration> builder)
        {
            builder.Ignore(tac => tac.TokenAlphas);
        }
    }
}
