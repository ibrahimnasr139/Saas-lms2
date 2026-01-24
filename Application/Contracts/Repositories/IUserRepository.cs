using Application.Features.Users.Dtos;

namespace Application.Contracts.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserTenantsDto>> GetTenantsAsync(string userId, CancellationToken cancellationToken);
    }
}
