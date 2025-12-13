using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Contracts.Caching
{
    public interface ICacheInvalidator
    {
        Task InvalidateLastTenantCache(string userId, CancellationToken cancellationToken = default);
        Task InvalidateUserTenantsCache(string userId, CancellationToken cancellationToken = default);
    }
}
