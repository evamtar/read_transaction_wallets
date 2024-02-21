using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Infra.Data.Base.Mapper;

namespace SyncronizationBot.Infra.Data.MongoDB.Mapper
{
    public class TokenAlphaWalletMap : BaseMapper<TokenAlphaWallet>
    {
        public TokenAlphaWalletMap() : base(EDatabase.Mongodb)
        {
        }

        protected override void IgnoreProperties(EntityTypeBuilder<TokenAlphaWallet> builder)
        {
            builder.Ignore(taw => taw.TokenAlpha);
            builder.Ignore(taw => taw.Wallet);
        }
        
    }
}
