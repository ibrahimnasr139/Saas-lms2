using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    internal sealed class RolePermissionConfigurations : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.HasKey(builder => new { builder.TenantRoleId, builder.PermissionId });
            builder.HasOne(builder => builder.TenantRole)
                .WithMany(role => role.RolePermissions)
                .HasForeignKey(builder => builder.TenantRoleId);
            builder.HasOne(builder => builder.Permission)
                .WithMany()
                .HasForeignKey(builder => builder.PermissionId);


        }
    }
}
