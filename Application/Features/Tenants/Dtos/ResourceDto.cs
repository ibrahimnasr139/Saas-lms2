
namespace Application.Features.Tenants.Dtos
{
    public sealed class ResourceDto
    {
        public List<DocumentDto> Documents { get; set; } = [];
        public List<VideoDto> Videos { get; set; } = [];
        public List<ImageDto> Images { get; set; } = [];
    }
}
