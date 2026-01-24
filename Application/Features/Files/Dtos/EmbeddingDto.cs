namespace Application.Features.Files.Dtos
{
    public sealed record EmbeddingDto(bool Enabled, int ChunckSize, int ChunkOverLap);
}
