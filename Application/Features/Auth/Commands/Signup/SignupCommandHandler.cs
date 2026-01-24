
using Application.Constants;
using Application.Helpers;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Application.Features.Auth.Commands.Signup
{
    internal sealed class SignupCommandHandler : IRequestHandler<SignupCommand, OneOf<bool, Error>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly HybridCache _hybridCache;
        private readonly ILogger<SignupCommandHandler> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SignupCommandHandler(
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            IEmailSender emailSender,
            HybridCache hybridCache,
            ILogger<SignupCommandHandler> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _mapper = mapper;
            _emailSender = emailSender;
            _hybridCache = hybridCache;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<OneOf<bool, Error>> Handle(SignupCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser is not null)
            {
                if (existingUser.EmailConfirmed)
                    return UserErrors.UserAlreadyExists;
            }
            else
            {
                var newUser = _mapper.Map<ApplicationUser>(request);
                var createUserResult = await _userManager.CreateAsync(newUser, request.Password);
                if (!createUserResult.Succeeded)
                {
                    var error = string.Join(", ", createUserResult.Errors.Select(e => e.Description).First());
                    return new Error("UserCreationFailed", error, HttpStatusCode.BadRequest);
                }
                await _userManager.AddToRoleAsync(newUser, RoleConstants.Tenant);
            }
            var otpCode = await GenerateOtpHelper.GenerateOtp(request.Email, _hybridCache, _httpContextAccessor, cancellationToken);
            var emailBody = EmailConfirmationHelper.GenerateEmailBodyHelper(EmailConstants.OtpTemplate, new Dictionary<string, string>
            {
                { "{{OTP_CODE}}", otpCode },
                { "{{UserName}}", request.FirstName }
            });
            _logger.LogInformation("Enqueuing email confirmation job for {Email} and otp is {otpCode}", request.Email, otpCode);
            BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(request.Email, EmailConstants.EmailConfirmationSubject, emailBody));
            return true;
        }
    }
}
