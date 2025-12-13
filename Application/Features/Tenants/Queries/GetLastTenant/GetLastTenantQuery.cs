using Application.Features.Tenants.Dtos;

namespace Application.Features.Tenants.Queries.GetLastTenant
{
    public sealed record GetLastTenantQuery : IRequest<LastTenantDto?>;
}
