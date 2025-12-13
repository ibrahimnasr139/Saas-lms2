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

            CreateMap<TenantMember, UserTenantsDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Tenant.Id))
                .ForMember(dest => dest.PlatformName, opt => opt.MapFrom(src => src.Tenant.PlatformName))
                .ForMember(dest => dest.Logo, opt => opt.MapFrom(src => src.Tenant.Logo))
                .ForMember(dest => dest.SubDomain, opt => opt.MapFrom(src => src.Tenant.SubDomain))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.TenantRole.Name));
        }
    }
}
