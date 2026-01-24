using Domain.Abstractions;
using Domain.Enums;

namespace Domain.Entites
{
    public sealed class PlanPricing : IAuditable
    {
        public Guid Id { get; set; }
        public BillingCycle BillingCycle { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; } = string.Empty;
        public decimal? DiscountPercent { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public Guid PlanId { get; set; }
        public Plan Plan { get; set; } = null!;
        public ICollection<Subscription> Subscriptions { get; set; } = [];
    }
}
