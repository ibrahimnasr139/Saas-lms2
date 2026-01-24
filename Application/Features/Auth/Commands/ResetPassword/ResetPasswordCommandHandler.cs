namespace Application.Features.Auth.Commands.ResetPassword
{
    internal sealed class ResetPasswordCommandHandler :
        IRequestHandler<ResetPasswordCommand, OneOf<bool, Error>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly HybridCache _hybridCache;
        public ResetPasswordCommandHandler(
            UserManager<ApplicationUser> userManager,
            HybridCache hybridCache)
        {
            _userManager = userManager;
            _hybridCache = hybridCache;
        }

        public async Task<OneOf<bool, Error>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var userId = await _hybridCache.GetOrCreateAsync(request.resetToken, async entry =>
            {
                return await Task.FromResult<string?>(null);
            }, cancellationToken: cancellationToken);
            if (string.IsNullOrEmpty(userId))
            {
                return UserErrors.InvalidOtpCode;
            }
            await _hybridCache.RemoveAsync(request.resetToken, cancellationToken);
            var user = await _userManager.FindByIdAsync(userId);

            var token = await _userManager.GeneratePasswordResetTokenAsync(user!);
            var result = await _userManager.ResetPasswordAsync(user!, token, request.Password);
            if (!result.Succeeded)
            {
                throw new Exception("Something went wrong");
            }
            return true;
        }
    }
}
