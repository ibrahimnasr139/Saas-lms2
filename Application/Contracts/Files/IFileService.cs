using Domain.Enums;

namespace Application.Contracts.Files
{
    public interface IFileService
    {
        long GetMaxSize(FileType fileType);
        string GetPath(string fileId, string name, string folder);
        string CreateUploadUrl(string path);
        string CreateCdnUrl(string path);
    }
}
