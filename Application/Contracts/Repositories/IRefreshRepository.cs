using Domain.Entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Contracts.Repositories
{
    public interface IRefreshRepository
    {
        Task<RefreshToken?> GetRefreshTokenAsync(string token, CancellationToken cancellationToken);
        void AddRefreshToken(ApplicationUser user, string token, DateTime expiresAt, CancellationToken cancellationToken);
        Task<int> SaveAsync(CancellationToken cancellationToken);

    }
}
