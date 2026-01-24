using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    internal sealed class TeachingLevelConfigurations : IEntityTypeConfiguration<TeachingLevel>
    {
        public void Configure(EntityTypeBuilder<TeachingLevel> builder)
        {
            builder.Property(g => g.Value)
                 .HasMaxLength(100)
                 .IsRequired();
            builder.Property(g => g.Label)
                     .HasMaxLength(100)
                     .IsRequired();
        }
    }
}
