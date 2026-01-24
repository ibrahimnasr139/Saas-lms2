using Domain.Abstractions;
using Domain.Enums;

namespace Domain.Entites
{
    public sealed class Subscription : IAuditable
    {
        public int Id { get; set; }
        public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Active;
        public string? ProviderSubscriptionId { get; set; }
        public string? Provider { get; set; }
        public bool IsAutoRenew { get; set; }
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; } = null!;
        public Guid PlanPricingId { get; set; }
        public PlanPricing PlanPricing { get; set; } = null!;
        public DateTime StartsAt { get; set; }
        public DateTime EndsAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
