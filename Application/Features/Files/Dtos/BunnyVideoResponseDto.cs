using System.Text.Json.Serialization;

namespace Application.Features.Files.Dtos
{
    public sealed class BunnyVideoResponseDto
    {
        [JsonPropertyName("guid")]
        public string Guid { get; set; } = string.Empty;

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("libraryId")]
        public int LibraryId { get; set; }
    }
}
