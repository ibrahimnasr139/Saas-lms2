namespace Application.Features.Files.Dtos
{
    public sealed class CreateUploadDto
    {
        public string VideoId { get; set; } = string.Empty;
        public string LibraryId { get; set; } = string.Empty;
        public long ExpirationTime { get; set; }
        public string Signature { get; set; } = string.Empty;
        public string EmbedUrl { get; set; } = string.Empty;
    }
}
