using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;


namespace SyncronizationBot.Infra.Data.Mapper
{
    public class TelegramChannelMap : IEntityTypeConfiguration<TelegramChannel>
    {
        public void Configure(EntityTypeBuilder<TelegramChannel> builder)
        {
            builder.ToTable("TelegramChannel");
            builder.Property(cw => cw.ID);
            builder.Property(cw => cw.ChannelId).HasPrecision(30,0);
            builder.Property(cw => cw.ChannelName);
            builder.Property(cw => cw.TimeBeforeDelete);
            builder.HasKey(cw => cw.ID);
        }
    }
}
