using Application.Common;
using Application.Features.Users.Dtos;

namespace Application.Features.Users.Queries.GetProfile
{
    internal sealed class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, UserProfileDto>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICurrentUserId _currentUserId;
        private readonly IMapper _mapper;
        public GetProfileQueryHandler(UserManager<ApplicationUser> userManager, ICurrentUserId currentUserId
            , IMapper mapper)
        {
            _userManager = userManager;
            _currentUserId = currentUserId;
            _mapper = mapper;
        }

        public async Task<UserProfileDto> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserId.GetUserId();
            var user = await _userManager.FindByIdAsync(userId!);
            var userProfileDto = _mapper.Map<UserProfileDto>(user);
            var roles = await _userManager.GetRolesAsync(user!);
            userProfileDto.Role = roles.FirstOrDefault() ?? string.Empty;
            return userProfileDto;
        }
    }
}
