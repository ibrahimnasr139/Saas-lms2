using Application.Constants;
using Application.Contracts.Caching;
using Microsoft.Extensions.Caching.Hybrid;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Caching
{
    internal sealed class CacheInvalidator : ICacheInvalidator
    {
        private readonly HybridCache _hybridCache;

        public CacheInvalidator(HybridCache hybridCache)
        {
            _hybridCache = hybridCache;
        }
        public async Task InvalidateLastTenantCache(string userId, CancellationToken cancellationToken = default)
        {
            var cacheKey = $"{CacheKeysConstants.LastTenantKey}_{userId}";
            await _hybridCache.RemoveAsync(cacheKey, cancellationToken);
        }

        public async Task InvalidateUserTenantsCache(string userId, CancellationToken cancellationToken = default)
        {
            var cacheKey = $"{CacheKeysConstants.UserTenantsKey}_{userId}";
            await _hybridCache.RemoveAsync(cacheKey, cancellationToken);
        }
    }
}
