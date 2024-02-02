using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;


namespace SyncronizationBot.Infra.Data.Mapper
{
    public class TelegramMessageMap : IEntityTypeConfiguration<TelegramMessage>
    {
        public void Configure(EntityTypeBuilder<TelegramMessage> builder)
        {
            builder.ToTable("TelegramMessage");
            builder.Property(tm => tm.ID);
            builder.Property(tm => tm.MessageId);
            builder.Property(tm => tm.TelegramChannelId);
            builder.Property(tm => tm.DateSended);
            builder.Property(tm => tm.IsDeleted);
            builder.HasOne(tm => tm.TelegramChannel).WithMany(tc => tc.TelegramMessages).HasForeignKey(tm => tm.TelegramChannelId);
            builder.HasKey(tm => tm.ID);
        }
    }
}
