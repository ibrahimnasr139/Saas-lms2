using Application.Features.Plan.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Contracts.Repositories
{
    public interface IPlanRepository
    {
        Task<IEnumerable<PlanDto>> GetAllPlansWithDetailsAsync(CancellationToken cancellationToken);
        Task<Guid> GetFreePlanPricingIdAsync(CancellationToken cancellationToken);

    }
}
