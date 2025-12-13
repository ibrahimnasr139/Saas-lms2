using Application.Contracts.Repositories;
using Application.Features.Tenants.Dtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Domain.Constants;
using Application.Constants;
using Application.Common;
using Application.Contracts.Caching;

namespace Application.Features.Tenants.Commands.CreateOnboarding
{
    internal sealed class CreateOnboardingCommandHandler : IRequestHandler<CreateOnboardingCommand, OneOf<OnboardingDto, Error>>
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICurrentUserId _currentUserId;
        private readonly ICacheInvalidator _cacheInvalidator;

        public CreateOnboardingCommandHandler(ITenantRepository tenantRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor
            , UserManager<ApplicationUser> userManager, ICurrentUserId currentUserId, ICacheInvalidator cacheInvalidator)
        {
            _tenantRepository = tenantRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _currentUserId = currentUserId;
            _cacheInvalidator = cacheInvalidator;
        }

        public async Task<OneOf<OnboardingDto, Error>> Handle(CreateOnboardingCommand request, CancellationToken cancellationToken)
        {
            var isSubDomainExists = await _tenantRepository.IsSubDomainExistsAsync(request.SubDomain, cancellationToken);
            if (isSubDomainExists)
            {
                return TenantErrors.SubDomainAlreadyExists;
            }

            await _tenantRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                var ownerId = _currentUserId.GetUserId();
                var user = await _userManager.FindByIdAsync(ownerId!);

                var tenant = _mapper.Map<Tenant>(request);
                tenant.OwnerId = ownerId!;
                var createdTenantId = await _tenantRepository.CreateTenantAsync(tenant, cancellationToken);

                _mapper.Map(request, user!);

                await _tenantRepository.AddTenantRoles(createdTenantId, cancellationToken);
                await _tenantRepository.SaveAsync(cancellationToken);

                var tenantMember = _mapper.Map<TenantMember>(request);
                tenantMember.UserId = ownerId!;
                var tenantRole = await _tenantRepository.FindTenantRoleByTenantId(createdTenantId, RolesConstants.Owner, cancellationToken);
                tenantMember.TenantRole = tenantRole!;
                tenantMember.TenantId = createdTenantId;
                await _tenantRepository.AddTenantMemberAsync(tenantMember, cancellationToken);
                await _userManager.AddToRoleAsync(user!, RolesConstants.Owner);
                
                await _tenantRepository.CommitTransactionAsync(cancellationToken);

                await _cacheInvalidator.InvalidateLastTenantCache(ownerId!, cancellationToken);
                await _cacheInvalidator.InvalidateUserTenantsCache(ownerId!, cancellationToken);

                _httpContextAccessor?.HttpContext?.Response.Cookies.Append(AuthConstants.SubDomain, request.SubDomain, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None
                });

                return new OnboardingDto
                {
                    SubDomain = request.SubDomain
                };
            }
            catch
            {
                await _tenantRepository.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }
    }
}
