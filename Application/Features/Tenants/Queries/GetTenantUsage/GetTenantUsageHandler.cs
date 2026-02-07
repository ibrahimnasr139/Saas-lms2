using Application.Constants;
using Application.Contracts.Repositories;
using Application.Features.Tenants.Dtos;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Tenants.Queries.GetTenantUsage
{
    internal sealed class GetTenantUsageHandler : IRequestHandler<GetTenantUsageQuery, TenantUsageDto>
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly ITenantMemberRepository _tenantMemberRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetTenantUsageHandler(ITenantRepository tenantRepository, ITenantMemberRepository tenantMemberRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _tenantRepository = tenantRepository;
            _tenantMemberRepository = tenantMemberRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<TenantUsageDto> Handle(GetTenantUsageQuery request, CancellationToken cancellationToken)
        {
            var subdomain = _httpContextAccessor.HttpContext?.Request.Cookies[AuthConstants.SubDomain];
            var tenantId = await _tenantRepository.GetTenantIdAsync(subdomain!, cancellationToken);
            return await _tenantRepository.GetTenantUsageAsync(tenantId, cancellationToken);
        }
    }
}
