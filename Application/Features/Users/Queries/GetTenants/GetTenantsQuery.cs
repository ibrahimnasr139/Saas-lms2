using Application.Features.Users.Dtos;

namespace Application.Features.Users.Queries.GetTenants
{
    public sealed record GetTenantsQuery : IRequest<IEnumerable<UserTenantsDto>>;
}
