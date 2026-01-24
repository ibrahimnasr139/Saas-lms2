namespace Application.Contracts.Repositories
{
    public interface ISubscriptionRepository
    {
        Task CreateFreeSubcscription(int TenantId, Guid PlanPricingId, CancellationToken cancellationToken);
    }
}
