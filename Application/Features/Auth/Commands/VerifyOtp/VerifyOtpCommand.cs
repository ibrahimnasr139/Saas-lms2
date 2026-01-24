namespace Application.Features.Auth.Commands.VerifyOtp
{
    public sealed record VerifyOtpCommand(string OtpCode)
        : IRequest<OneOf<bool, Error>>;
}
