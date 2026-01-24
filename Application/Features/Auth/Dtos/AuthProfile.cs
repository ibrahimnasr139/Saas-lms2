using Application.Features.Auth.Commands.Signup;

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
