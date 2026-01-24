using Application.Constants;
using Application.Helpers;
using Hangfire;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Auth.Commands.ResendOtp
{
    internal sealed class ResendOtpCommandHandler : IRequestHandler<ResendOtpCommand, OneOf<bool, Error>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HybridCache _hybridCache;
        public ResendOtpCommandHandler(
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender,
            IHttpContextAccessor httpContextAccessor,
            HybridCache hybridCache)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _httpContextAccessor = httpContextAccessor;
            _hybridCache = hybridCache;
        }
        public async Task<OneOf<bool, Error>> Handle(ResendOtpCommand request, CancellationToken cancellationToken)
        {
            if (!_httpContextAccessor!.HttpContext!.Request.Cookies.TryGetValue(AuthConstants.VerificationCode, out string code))
            {
                return UserErrors.InvalidVerificationToken;
            }
            var email = await _hybridCache.GetOrCreateAsync<string>(code, async entry =>
            {
                return await Task.FromResult(string.Empty);
            }, cancellationToken: cancellationToken);
            var otpCode = new Random().Next(100000, 999999).ToString();
            await _hybridCache.SetAsync(email, otpCode, new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromMinutes(2)
            }, cancellationToken: cancellationToken);

            var emailBody = EmailConfirmationHelper.GenerateEmailBodyHelper(EmailConstants.OtpTemplate, new Dictionary<string, string>
            {
                { "{{OTP_CODE}}", otpCode },
                { "{{UserName}}", "user" }
            });
            BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(email, EmailConstants.EmailConfirmationSubject, emailBody));
            return true;



        }
    }
}
