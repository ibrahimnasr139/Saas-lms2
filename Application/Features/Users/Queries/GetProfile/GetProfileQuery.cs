using Application.Features.Users.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Users.Queries.GetProfile
{
    public sealed record GetProfileQuery : IRequest<UserProfileDto>;
}
