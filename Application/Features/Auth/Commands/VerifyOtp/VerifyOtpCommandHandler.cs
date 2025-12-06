using Application.Contracts.Authentication;
using Application.Contracts.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Commands.VerifyOtp
{
    internal sealed class VerifyOtpCommandHandler : IRequestHandler<VerifyOtpCommand, OneOf<bool, Error>>
    {
        private readonly HybridCache _hybridCache;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRefreshRepository _refreshRepository;
        private readonly ITokenProvider _tokenProvider;
        public VerifyOtpCommandHandler(HybridCache hybridCache, UserManager<ApplicationUser> userManager,
            IRefreshRepository refreshRepository, ITokenProvider tokenProvider)
        {
            _hybridCache = hybridCache;
            _userManager = userManager;
            _refreshRepository = refreshRepository;
            _tokenProvider = tokenProvider;
        }
        public async Task<OneOf<bool, Error>> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
        {
            var cacheKey = $"EmailOtp_{request.Email}";
            var cachedOtp = await _hybridCache.GetOrCreateAsync(cacheKey, async entry =>
            {
               return await Task.FromResult<string?>(null);
            }, cancellationToken: cancellationToken);
            if (cachedOtp is null || !string.Equals(cachedOtp, request.OtpCode, StringComparison.Ordinal))
            {
                return UserErrors.InvalidOtpCode;
            }

            await _hybridCache.RemoveAsync(cacheKey);
            var user = await _userManager.FindByEmailAsync(request.Email);
            user?.EmailConfirmed = true;
            _tokenProvider.GenerateJwtToken(user!);
            var refreshToken = _tokenProvider.GenerateRefreshToken();
            _refreshRepository.AddRefreshToken(user!, refreshToken.token, refreshToken.expiresOn, cancellationToken);
            await _refreshRepository.SaveAsync(cancellationToken);
            return true;
        }
    }
}
