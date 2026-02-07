using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    internal sealed class TenantUsageConfigurations : IEntityTypeConfiguration<TenantUsage>
    {
        public void Configure(EntityTypeBuilder<TenantUsage> builder)
        {
            builder.HasOne(tu => tu.PlanFeature)
           .WithMany(pf => pf.TenantUsages)
           .HasForeignKey(tu => tu.PlanFeatureId)
           .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(tu => tu.Tenant)
                   .WithMany(t => t.TenantUsages)
                   .HasForeignKey(tu => tu.TenantId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(tu => tu.Subscription)
                   .WithMany(s => s.TenantUsages)
                   .HasForeignKey(tu => tu.SubscriptionId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(tu => new
            {
                tu.TenantId,
                tu.SubscriptionId,
                tu.PlanFeatureId
            }).IsUnique();
        }
    }
}
