using Application.Features.Roles.Dtos;

namespace Application.Contracts.Repositories
{
    public interface ITenantRoleRepository
    {
        Task<List<TenantRolesDto>> GetTenantRolesAsync(int tenantId, CancellationToken cancellationToken);
    }
}
