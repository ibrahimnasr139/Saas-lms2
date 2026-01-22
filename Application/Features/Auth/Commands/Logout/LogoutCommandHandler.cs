using Application.Constants;
using Application.Contracts.Repositories;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Auth.Commands.Logout
{
    internal sealed class LogoutCommandHandler : IRequestHandler<LogoutCommand, OneOf<bool, Error>>
    {
        private readonly IRefreshRepository _refreshRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LogoutCommandHandler(IRefreshRepository refreshRepository, IHttpContextAccessor httpContextAccessor)
        {
            _refreshRepository = refreshRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<OneOf<bool, Error>> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext is null)
            {
                throw new InvalidOperationException("HTTP context is not available.");
            }
            var refreshToken = httpContext.Request.Cookies[AuthConstants.RefreshToken];
            var token = await _refreshRepository.GetRefreshTokenAsync(refreshToken!, cancellationToken);
            if (token is not null)
            {
                token.RevokedAt = DateTime.UtcNow;
            }
            await _refreshRepository.SaveAsync(cancellationToken);
            httpContext.Response.Cookies.Delete(AuthConstants.AccessToken, new CookieOptions { Domain = AuthConstants.CookieDomain });
            httpContext.Response.Cookies.Delete(AuthConstants.RefreshToken, new CookieOptions { Domain = AuthConstants.CookieDomain });
            return true;
        }
    }
}
