using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    internal sealed class TenantMemberConfigurations : IEntityTypeConfiguration<TenantMember>
    {
        public void Configure(EntityTypeBuilder<TenantMember> builder)
        {
            builder.Property(tm => tm.ExperienceYears)
                   .HasMaxLength(100)
                   .IsRequired();
            builder.Property(tm => tm.JobTitle)
                   .HasMaxLength(100)
                   .IsRequired();
            builder.Property(tm => tm.Bio)
                   .HasMaxLength(1000);
            builder.Property(tm => tm.DisplayName)
                   .HasMaxLength(200)
                   .IsRequired();
            builder.Property(tm => tm.IsActive)
                   .IsRequired();

            builder.HasOne(tm => tm.TenantRole)
                   .WithMany(t => t.TenantMembers)
                   .HasForeignKey(tm => tm.TenantRoleId);
        }
    }
}
