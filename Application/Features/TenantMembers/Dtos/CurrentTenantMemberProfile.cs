namespace Application.Features.TenantMembers.Dtos
{
    public sealed class CurrentTenantMemberProfile : Profile
    {
        public CurrentTenantMemberProfile()
        {
            CreateMap<TenantMember, CurrentTenantMemberDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber))
                .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => src.User.ProfilePicture))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.TenantRole.Name))
                .ForMember(dest => dest.HasFullAccess, opt => opt.MapFrom(src => src.TenantRole.HasAllPermissions));
        }
    }
}
