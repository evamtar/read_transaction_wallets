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

        protected override void PropertiesWithConversion(EntityTypeBuilder<TokenAlphaWallet> builder)
        {
            //builder.Property(taw => taw.ValueSpentSol).HasConversion<string?>();
            //builder.Property(taw => taw.ValueSpentUSD).HasConversion<string?>();
            //builder.Property(taw => taw.QuantityToken).HasConversion<string?>();
            //builder.Property(taw => taw.ValueReceivedSol).HasConversion<string?>();
            //builder.Property(taw => taw.ValueReceivedUSD).HasConversion<string?>();
            //builder.Property(taw => taw.QuantityTokenSell).HasConversion<string?>();
        }

        protected override void RelationsShips(EntityTypeBuilder<TokenAlphaWallet> builder)
        {
            builder.Ignore(taw => taw.TokenAlpha);
            builder.Ignore(taw => taw.Wallet);

        }
        
    }
}
