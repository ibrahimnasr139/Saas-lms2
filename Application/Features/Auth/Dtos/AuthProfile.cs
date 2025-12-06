using Application.Features.Auth.Commands.Signup;
using Domain.Entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Dtos
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<SignupCommand, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
            CreateMap<ApplicationUser, LoginDto>()
                .ForMember(dest => dest.LastActiveTenant, opt => opt.MapFrom(src => src.LastActiveTenantSubDomain ?? null!));

        }
    }
}
