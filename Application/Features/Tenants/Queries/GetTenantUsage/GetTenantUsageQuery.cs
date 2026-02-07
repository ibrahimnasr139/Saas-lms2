using Application.Features.Tenants.Dtos;

namespace Application.Features.Tenants.Queries.GetTenantUsage
{
    public record GetTenantUsageQuery : IRequest<TenantUsageDto>
    {
    }
}
