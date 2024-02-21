using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Infra.Data.Base.Mapper;


namespace SyncronizationBot.Infra.Data.MongoDB.Mapper
{
    public class TokenPriceHistoryMap : BaseMapper<TokenPriceHistory>
    {
        public TokenPriceHistoryMap() : base(EDatabase.Mongodb)
        {
        }

        protected override void IgnoreProperties(EntityTypeBuilder<TokenPriceHistory> builder)
        {
            builder.Ignore(tph => tph.Token);
        }

    }
}
