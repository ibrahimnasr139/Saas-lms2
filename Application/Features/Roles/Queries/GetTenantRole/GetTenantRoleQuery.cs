using Application.Features.Roles.Dtos;

namespace Application.Features.Roles.Queries.GetTenantRole
{
    public sealed record GetTenantRoleQuery : IRequest<List<TenantRolesDto>> { }
}
