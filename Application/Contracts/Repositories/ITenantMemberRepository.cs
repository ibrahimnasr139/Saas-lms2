using Application.Features.TenantMembers.Dtos;

namespace Application.Contracts.Repositories
{
    public interface ITenantMemberRepository
    {
        Task<List<string>?> GetAllPermissions(int tenantRoleId, CancellationToken cancellationToken);
        Task<CurrentTenantMemberDto?> GetCurrentTenantMemberAsync(string userId, CancellationToken cancellationToken);
        Task<List<TenantMembersDto>> GetTenantMembersAsync(int tenantId, CancellationToken cancellationToken);
    }
}
