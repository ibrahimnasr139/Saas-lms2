using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    internal sealed class TenantInviteConfigurations : IEntityTypeConfiguration<TenantInvite>
    {
        public void Configure(EntityTypeBuilder<TenantInvite> builder)
        {
            builder.HasIndex(ti => ti.Token).IsUnique();

            builder.Property(ti => ti.Token)
                   .HasMaxLength(64)
                   .IsRequired();

            builder.Property(ti => ti.Email)
                   .HasMaxLength(256)
                   .IsRequired();

            builder.Property(ti => ti.Status)
                   .IsRequired();

            builder.Property(ti => ti.CreatedAt)
                   .IsRequired();

            builder.Property(ti => ti.ExpiresAt)
                   .IsRequired();

            builder.HasOne(ti => ti.Tenant)
                   .WithMany(t => t.TenantInvites)
                   .HasForeignKey(ti => ti.TenantId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ti => ti.TenantRole)
                   .WithMany(tr => tr.TenantInvites)
                   .HasForeignKey(ti => ti.TenantRoleId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ti => ti.TenantMember)
                   .WithMany(tm => tm.TenantInvites)
                   .HasForeignKey(ti => ti.InvitedBy)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
