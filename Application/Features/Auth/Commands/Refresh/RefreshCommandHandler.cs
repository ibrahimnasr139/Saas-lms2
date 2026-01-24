using Application.Contracts.Authentication;
using Application.Contracts.Repositories;

namespace Application.Features.Auth.Commands.Refresh
{
    internal sealed class RefreshCommandHandler : IRequestHandler<RefreshCommand, OneOf<bool, Error>>
    {
        private readonly IRefreshRepository _refreshRepository;
        private readonly ITokenProvider _tokenProvider;
        private readonly UserManager<ApplicationUser> _userManager;
        public RefreshCommandHandler(IRefreshRepository refreshRepository, ITokenProvider tokenProvider, UserManager<ApplicationUser> userManager)
        {
            _refreshRepository = refreshRepository;
            _tokenProvider = tokenProvider;
            _userManager = userManager;
        }
        public async Task<OneOf<bool, Error>> Handle(RefreshCommand request, CancellationToken cancellationToken)
        {
            var refreshToken = await _refreshRepository.GetRefreshTokenAsync(request.RefreshToken, cancellationToken);
            if (refreshToken is null || !refreshToken.IsActive)
            {
                return UserErrors.Unauthorized;
            }
            var user = await _userManager.FindByIdAsync(refreshToken.UserId.ToString());
            if (user is null)
            {
                return UserErrors.Unauthorized;
            }
            refreshToken.RevokedAt = DateTime.UtcNow;
            _tokenProvider.GenerateJwtToken(user);
            var newRefreshToken = _tokenProvider.GenerateRefreshToken();
            _refreshRepository.AddRefreshToken(user, newRefreshToken.token, newRefreshToken.expiresOn, cancellationToken);
            await _refreshRepository.SaveAsync(cancellationToken);
            return true;
        }
    }
}
