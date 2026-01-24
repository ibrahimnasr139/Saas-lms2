using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    internal sealed class PlanConfigurations : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(p => p.Description)
                   .HasMaxLength(1000)
                   .IsRequired();

            builder.Property(p => p.Slug)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.HasIndex(p => p.Slug)
                   .IsUnique();

            builder.Property(p => p.CreatedAt)
                   .IsRequired();

            builder.Property(p => p.UpdatedAt);
        }
    }
}
