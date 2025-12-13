using Application.Features.Users.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserTenantsDto>> GetTenantsAsync(string userId, CancellationToken cancellationToken);
    }
}
