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

        public async Task<bool> HasActiveSubscriptionByTenantDomain(string subdomain, CancellationToken cancellationToken)
        {
            return await (from s in _context.Subscriptions
                            join t in _context.Tenants on s.TenantId equals t.Id
                            where t.SubDomain == subdomain &&
                                    (s.Status == SubscriptionStatus.Active || 
                                    s.Status == SubscriptionStatus.Trialed)
                                    && s.EndsAt > DateTime.UtcNow
                           select s).AnyAsync(cancellationToken);
        }
    }
}
