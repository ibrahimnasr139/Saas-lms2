namespace Application.Features.Auth.Commands.Logout
{
    public sealed record LogoutCommand : IRequest<OneOf<bool, Error>>;
}
