using Microsoft.EntityFrameworkCore;

namespace Application.Contracts.Repositories
{
    public interface ITenantInviteRepository
    {
        Task CreateTenantInviteAsync(TenantInvite tenantInvite, CancellationToken cancellationToken);
        Task<int> SaveAsync(CancellationToken cancellationToken);
    }
}
