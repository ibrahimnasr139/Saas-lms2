namespace Application.Features.Auth.Commands.ResendOtp
{
    public sealed record ResendOtpCommand : IRequest<OneOf<bool, Error>>;
}
