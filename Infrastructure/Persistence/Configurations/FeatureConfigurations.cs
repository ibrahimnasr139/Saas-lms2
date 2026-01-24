using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    internal sealed class FeatureConfigurations : IEntityTypeConfiguration<Feature>
    {
        public void Configure(EntityTypeBuilder<Feature> builder)
        {
            builder.HasKey(f => f.Id);

            builder.Property(f => f.Name)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(f => f.Key)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.HasIndex(f => f.Key)
                   .IsUnique();

            builder.Property(f => f.CreatedAt)
                   .IsRequired();

            builder.Property(f => f.UpdatedAt);
        }
    }
}
