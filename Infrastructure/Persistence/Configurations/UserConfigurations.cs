using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    internal sealed class UserConfigurations : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(u => u.FirstName)
                   .HasMaxLength(100)
                   .IsRequired();
            builder.Property(u => u.LastName)
                   .HasMaxLength(100)
                   .IsRequired();
            builder.Property(u => u.LastActiveTenantSubDomain)
                   .HasMaxLength(200);
            builder.Property(u => u.HasOnboarded)
                   .IsRequired();
            builder.Property(u => u.IsSubscribed)
                   .IsRequired();
            builder.Property(u => u.CreatedAt)
                   .IsRequired();
            builder.Property(u => u.UpdatedAt);
        }
    }
}
