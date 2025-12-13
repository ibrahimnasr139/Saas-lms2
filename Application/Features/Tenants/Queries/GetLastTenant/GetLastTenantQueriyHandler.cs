using Application.Common;
using Application.Constants;
using Application.Contracts.Repositories;
using Application.Features.Tenants.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Tenants.Queries.GetLastTenant
{
    internal sealed class GetLastTenantQueriyHandler : IRequestHandler<GetLastTenantQueriy, LastTenantDto?>
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly ICurrentUserId _currentUserId;
        private readonly HybridCache _hybridCache;

        public GetLastTenantQueriyHandler(ITenantRepository tenantRepository, ICurrentUserId currentUserId, HybridCache hybridCache)
        {
            _tenantRepository = tenantRepository;
            _currentUserId = currentUserId;
            _hybridCache = hybridCache;
        }

        public async Task<LastTenantDto?> Handle(GetLastTenantQueriy request, CancellationToken cancellationToken)
        {
            var userId = _currentUserId.GetUserId();
            var cacheKey = $"{CacheKeysConstants.LastTenantKey}_{userId}";
            var tenant = await _hybridCache.GetOrCreateAsync(
                cacheKey,
                async ct => await _tenantRepository.GetLastTenantAsync(userId, ct),
                new HybridCacheEntryOptions
                {
                    Expiration = TimeSpan.FromHours(1),
                    LocalCacheExpiration = TimeSpan.FromHours(1)
                },
                cancellationToken: cancellationToken
            );
            return tenant;
        }
    }
}
