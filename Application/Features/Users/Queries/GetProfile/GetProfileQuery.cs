using Application.Features.Users.Dtos;

namespace Application.Features.Users.Queries.GetProfile
{
    public sealed record GetProfileQuery : IRequest<UserProfileDto>;
}
