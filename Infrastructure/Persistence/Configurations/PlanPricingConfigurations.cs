using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    internal sealed class PlanPricingConfigurations : IEntityTypeConfiguration<PlanPricing>
    {
        public void Configure(EntityTypeBuilder<PlanPricing> builder)
        {
            builder.HasKey(pp => pp.Id);

            builder.Property(pp => pp.Price)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(pp => pp.DiscountPercent)
                   .HasColumnType("decimal(5,2)");
            builder.Property(pp => pp.Currency)
                   .HasMaxLength(10)
                   .IsRequired();

            builder.Property(pp => pp.BillingCycle)
                    .HasConversion<string>()
                    .HasMaxLength(50)
                    .IsRequired();

            builder.Property(pp => pp.CreatedAt)
                   .IsRequired();

            builder.Property(pp => pp.UpdatedAt);

            builder.HasOne(pp => pp.Plan)
                   .WithMany(p => p.PlanPricings)
                   .HasForeignKey(pp => pp.PlanId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
