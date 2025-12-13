using Application.Features.Tenants.Dtos;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts.Repositories
{
    public interface ITenantRepository
    {
        Task<bool> IsSubDomainExistsAsync(string subDomain, CancellationToken cancellationToken);
        Task<int> CreateTenantAsync(Tenant tenant, CancellationToken cancellationToken);
        Task AddTenantRoles(int tenantId, CancellationToken cancellationToken);
        Task<TenantRole?> FindTenantRoleByTenantId(int tenantId, string roleName, CancellationToken cancellationToken);
        Task AddTenantMemberAsync(TenantMember member, CancellationToken cancellationToken);
        Task SaveAsync(CancellationToken cancellationToken);

        Task<LastTenantDto?> GetLastTenantAsync(string? subDomain, CancellationToken cancellationToken);


        Task BeginTransactionAsync(CancellationToken cancellationToken);
        Task CommitTransactionAsync(CancellationToken cancellationToken);
        Task RollbackTransactionAsync(CancellationToken cancellationToken);
    }
}
