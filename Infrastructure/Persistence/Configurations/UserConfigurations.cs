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
        }
    }
}
