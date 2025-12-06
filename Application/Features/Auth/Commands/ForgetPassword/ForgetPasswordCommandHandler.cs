using Application.Helpers;
using Hangfire;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Commands.ForgetPassword
{
    internal sealed class ForgetPasswordCommandHandler : IRequestHandler<ForgetPasswordCommand, OneOf<bool, Error>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly HybridCache _hybridCache;
        public ForgetPasswordCommandHandler(UserManager<ApplicationUser> userManager, IEmailSender emailSender, HybridCache hybridCache)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _hybridCache = hybridCache;
        }
        public async Task<OneOf<bool, Error>> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return UserErrors.EmailNotFound;
            }
            var otpCode = new Random().Next(100000, 999999).ToString();
            await _hybridCache.SetAsync(otpCode,user.Id, new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromMinutes(2)
            }, cancellationToken: cancellationToken);

            var emailBody = EmailConfirmationHelper.GenerateEmailBodyHelper("ForgetPasswordTemplate", new Dictionary<string, string>
            {
                { "{{name}}", user.FirstName },
                 { "{{action_url}}", $"/identity/forget-password?id={user.Id}&token={otpCode}" }
            });
            BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(user.Email!, "Reset Password", emailBody));
            return true;
        }
    }
}
