using DistributedLogging.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DistributedLogging.Presistence.Configs
{
    public class LoggedEntryConfiguration : IEntityTypeConfiguration<LoggedEntry>
    {
        public void Configure(EntityTypeBuilder<LoggedEntry> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Service).HasMaxLength(60);
            builder.Property(x => x.Level).HasMaxLength(60);
            builder.Property(x => x.Message).HasMaxLength(100);
            builder.Property(x => x.TimeStamp);
        }
    }
}
