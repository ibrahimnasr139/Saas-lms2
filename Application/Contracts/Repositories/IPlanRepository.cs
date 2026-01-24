using Application.Features.Plan.DTOs;

namespace Application.Contracts.Repositories
{
    public interface IPlanRepository
    {
        Task<IEnumerable<PlanDto>> GetAllPlansWithDetailsAsync(CancellationToken cancellationToken);
        Task<Guid> GetFreePlanPricingIdAsync(CancellationToken cancellationToken);

    }
}
