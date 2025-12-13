using Application.Features.Tenants.Dtos;

namespace Application.Features.Tenants.Queries.GetLastTenant
{
    public sealed record GetLastTenantQueriy() : IRequest<LastTenantDto?>;
}
