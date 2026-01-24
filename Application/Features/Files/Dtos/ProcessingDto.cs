namespace Application.Features.Files.Dtos
{
    public sealed record ProcessingDto(EmbeddingDto Embedding, TranscriptDto Transcript, bool? ExtractText);
}
