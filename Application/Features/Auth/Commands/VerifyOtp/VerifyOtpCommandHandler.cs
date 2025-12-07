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
            var email = await _hybridCache.GetOrCreateAsync(request.OtpCode, async entry =>
            {
               return await Task.FromResult<string?>(null);
            }, cancellationToken: cancellationToken);
            if (email is null)
            {
                return UserErrors.InvalidOtpCode;
            }

            await _hybridCache.RemoveAsync(email);
            var user = await _userManager.FindByEmailAsync(email);
            user?.EmailConfirmed = true;
            _tokenProvider.GenerateJwtToken(user!);
            var refreshToken = _tokenProvider.GenerateRefreshToken();
            _refreshRepository.AddRefreshToken(user!, refreshToken.token, refreshToken.expiresOn, cancellationToken);
            await _refreshRepository.SaveAsync(cancellationToken);
            return true;
        }
    }
}
