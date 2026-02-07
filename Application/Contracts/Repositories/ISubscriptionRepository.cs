namespace Application.Contracts.Repositories
{
    public interface ISubscriptionRepository
    {
        Task<int> CreateFreeSubcscription(int TenantId, Guid PlanPricingId, CancellationToken cancellationToken);
        Task<bool> HasActiveSubscriptionByTenantDomain(string subdomain, CancellationToken cancellationToken);


        Task<Guid> GetPlanPricingIdAsync(int tenantId, CancellationToken cancellationToken);
    }
}
