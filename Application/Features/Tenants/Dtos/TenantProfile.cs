using Application.Features.Tenants.Commands.CreateOnboarding;

namespace Application.Features.Tenants.Dtos
{
    public class TenantProfile : Profile
    {
        public TenantProfile()
        {
            CreateMap<LabelValueDto, Subject>();
            CreateMap<LabelValueDto, TeachingLevel>();
            CreateMap<LabelValueDto, Grade>();
            CreateMap<CreateOnboardingCommand, Tenant>();
            CreateMap<CreateOnboardingCommand, ApplicationUser>()
                .ForMember(dest => dest.LastActiveTenantSubDomain, opt => opt.MapFrom(src => src.SubDomain));
            CreateMap<CreateOnboardingCommand, TenantMember>();


            CreateMap<Tenant, LastTenantDto>()
                .ForMember(dest => dest.Subjects, opt => opt.MapFrom(src => src.Subjects))
                .ForMember(dest => dest.TeachingLevels, opt => opt.MapFrom(src => src.TeachingLevels))
                .ForMember(dest => dest.Grades, opt => opt.MapFrom(src => src.Grades));
            CreateMap<Subject, LabelValueIdDto>();
            CreateMap<TeachingLevel, LabelValueIdDto>();
            CreateMap<Grade, LabelValueIdDto>();

            CreateMap<Domain.Entites.File, DocumentDto>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.UploadedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.Size, opt => opt.MapFrom(src => src.Size / 1024))
                .ForMember(dest => dest.Extension, opt => opt.MapFrom(src => Path.GetExtension(src.Url).TrimStart('.')));

            CreateMap<Domain.Entites.File, ImageDto>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.UploadedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.Size, opt => opt.MapFrom(src => src.Size / 1024))
                .ForMember(dest => dest.Extension, opt => opt.MapFrom(src => Path.GetExtension(src.Url).TrimStart('.')));


            CreateMap<Domain.Entites.File, VideoDto>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.UploadedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.Size, opt => opt.MapFrom(src => src.Size / 1024))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Extension, opt => opt.MapFrom(src => Path.GetExtension(src.Url).TrimStart('.')));
        }
    }
}
