using Application.Common;
using Application.Constants;
using Application.Contracts.Repositories;
using Application.Features.Users.Dtos;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Users.Queries.GetProfile
{
    internal sealed class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, UserProfileDto>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICurrentUserId _currentUserId;
        private readonly IMapper _mapper;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public GetProfileQueryHandler(UserManager<ApplicationUser> userManager, ICurrentUserId currentUserId
            , IMapper mapper, ISubscriptionRepository subscriptionRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _currentUserId = currentUserId;
            _mapper = mapper;
            _subscriptionRepository = subscriptionRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UserProfileDto> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserId.GetUserId();
            var user = await _userManager.FindByIdAsync(userId!);
            var userProfileDto = _mapper.Map<UserProfileDto>(user);
            var roles = await _userManager.GetRolesAsync(user!);
            userProfileDto.Role = roles.FirstOrDefault() ?? string.Empty;
            if (user!.HasOnboarded)
            {
                var subdomain = _httpContextAccessor.HttpContext?.Request.Cookies[AuthConstants.SubDomain];
                userProfileDto.IsSubscribed = await _subscriptionRepository.HasActiveSubscriptionByTenantDomain(subdomain!, cancellationToken);
            }
            if (user.LastActiveTenantSubDomain is not null)
            {
                _httpContextAccessor?.HttpContext?.Response.Cookies.Append(AuthConstants.SubDomain, user.LastActiveTenantSubDomain, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Domain = AuthConstants.CookieDomain,
                    IsEssential = true
                });
            }
            return userProfileDto;
        }
    }
}
