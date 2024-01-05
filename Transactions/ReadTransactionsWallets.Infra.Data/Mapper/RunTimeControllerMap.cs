using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReadTransactionsWallets.Domain.Model.Database;


namespace ReadTransactionsWallets.Infra.Data.Mapper
{
    public class RunTimeControllerMap : IEntityTypeConfiguration<RunTimeController>
    {
        public void Configure(EntityTypeBuilder<RunTimeController> builder)
        {
            builder.ToTable("RunTimeController");
            builder.Property(rt => rt.IdRuntime);
            builder.Property(rt => rt.UnixTimeSeconds);
            builder.Property(rt => rt.IsRunning);
            builder.Ignore(rt => rt.ID);
            builder.HasNoKey();
        }
    }
}
