using Application.Features.Files.Commands.UploadFile;

namespace Application.Features.Files.Dtos
{
    public sealed class FileProfile : Profile
    {
        public FileProfile()
        {
            CreateMap<UploadFileCommand, Domain.Entites.File>();
        }

    }
}
