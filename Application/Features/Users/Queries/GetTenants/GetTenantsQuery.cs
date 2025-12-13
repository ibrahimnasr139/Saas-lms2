using Application.Features.Users.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Users.Queries.GetTenants
{
    public sealed record GetTenantsQuery() : IRequest<IEnumerable<UserTenantsDto>>;
}
