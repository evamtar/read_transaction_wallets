using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Infra.Data.Base.Mapper;

namespace SyncronizationBot.Infra.Data.SQLServer.Mapper
{
    public class TokenAlphaWalletMap : BaseMapper<TokenAlphaWallet>
    {
        public TokenAlphaWalletMap() : base(EDatabase.SqlServer)
        {
        }

        protected override void RelationsShips(EntityTypeBuilder<TokenAlphaWallet> builder)
        {
            builder.HasOne(taw => taw.TokenAlpha).WithMany(t => t.TokenAlphas).HasForeignKey(ta => ta.TokenAlphaId);
            builder.HasOne(taw => taw.Wallet).WithMany(t => t.TokenAlphas).HasForeignKey(ta => ta.WalletId);
        }
        
    }
}
