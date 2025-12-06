using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Users.Dtos
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, UserProfileDto>();

        }

    }
}
