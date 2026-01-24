using Domain.Enums;

namespace Infrastructure.Repositories
{
    internal class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly AppDbContext _context;
        public SubscriptionRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task CreateFreeSubcscription(int TenantId, Guid PlanPricingId, CancellationToken cancellationToken)
        {
            var subscription = new Subscription
            {
                TenantId = TenantId,
                PlanPricingId = PlanPricingId,
                StartsAt = DateTime.UtcNow,
                EndsAt = DateTime.UtcNow.AddDays(30),
                Status = SubscriptionStatus.Trialed
            };
            await _context.Subscriptions.AddAsync(subscription, cancellationToken);
        }
    }
}
