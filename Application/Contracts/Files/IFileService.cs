using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Application.Contracts.Files
{
    public interface IFileService
    {
        long GetMaxSize(FileType fileType);
        string GetPath(string fileId, string name, string? folder, string originalFileName);
        FileType GetFileType(string contentType);
        Task<string?> UploadAsync(IFormFile file, string? folder, string fileId, string path, bool embeddingEnabled, CancellationToken cancellationToken);
    }
}
