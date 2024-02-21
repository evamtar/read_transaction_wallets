using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Infra.Data.Base.Mapper;


namespace SyncronizationBot.Infra.Data.MongoDB.Mapper
{
    public class TokenMap : BaseMapper<Token>
    {
        public TokenMap() : base(EDatabase.Mongodb)
        {
        }

        protected override void IgnoreProperties(EntityTypeBuilder<Token> builder)
        {
            builder.Ignore(t => t.Balances);
            builder.Ignore(t => t.TokenSecurities);
            builder.Ignore(t => t.TokenAlphas);
            builder.Ignore(t => t.TransactionTokens);
            builder.Ignore(t => t.TokenPriceHistories);
}
    }
}
