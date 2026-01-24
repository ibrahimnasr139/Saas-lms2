using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    internal sealed class SubjectConfigurations : IEntityTypeConfiguration<Subject>
    {
        public void Configure(EntityTypeBuilder<Subject> builder)
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
