using Application.Constants;
using Application.Features.Auth.Commands.ForgetPassword;
using Application.Features.Auth.Commands.Login;
using Application.Features.Auth.Commands.Logout;
using Application.Features.Auth.Commands.Refresh;
using Application.Features.Auth.Commands.ResendOtp;
using Application.Features.Auth.Commands.ResetPassword;
using Application.Features.Auth.Commands.Signup;
using Application.Features.Auth.Commands.VerifyOtp;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("signup")]
        public async Task<IActionResult> Signup(SignupCommand signupCommand, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(signupCommand, cancellationToken);
            return result.Match(
                success => Created(),
                error => StatusCode((int)error.HttpStatusCode, error.Message)
            );
        }
        [HttpPost("resend-otp")]
        public async Task<IActionResult> ResendOtp(ResendOtpCommand resendOtpCommand, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(resendOtpCommand, cancellationToken);
            return result.Match(
                success => Ok(new { Message = "Success" }),
                error => StatusCode((int)error.HttpStatusCode, error.Message)
            );
        }
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp(VerifyOtpCommand verifyOtpCommand, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(verifyOtpCommand, cancellationToken);
            return result.Match(
                success => Ok(new { Message = "Success" }),
                error => StatusCode((int)error.HttpStatusCode, error.Message)
            );
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand loginCommand, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(loginCommand, cancellationToken);
            return result.Match(
                loginDto => Ok(loginDto),
                error => StatusCode((int)error.HttpStatusCode, error.Message)
            );
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgetPasswordCommand forgetPasswordCommand, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(forgetPasswordCommand, cancellationToken);
            return result.Match(
                success => Ok(new { Message = "success" }),
                error => StatusCode((int)error.HttpStatusCode, error.Message)
            );
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand resetPasswordCommand, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(resetPasswordCommand, cancellationToken);
            return result.Match(
                success => Ok(new { Message = "Password reset successfully" }),
                error => StatusCode((int)error.HttpStatusCode, error.Message)
            );
        }
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(CancellationToken cancellationToken)
        {
            var token = Request.Cookies[AuthConstants.RefreshToken];
            var refreshCommand = new RefreshCommand(string.IsNullOrEmpty(token) ? null : token.ToString());
            var result = await _mediator.Send(refreshCommand, cancellationToken);
            return result.Match(
                success => Ok(new { Message = "Success" }),
                error => StatusCode((int)error.HttpStatusCode, error.Message)
            );
        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout(CancellationToken cancellationToken)
        {
            var logoutCommand = new LogoutCommand();
            var result = await _mediator.Send(logoutCommand, cancellationToken);
            return result.Match(
                success => Ok(new { Message = "Success" }),
                error => StatusCode((int)error.HttpStatusCode, error.Message)
            );
        }
    }
}
