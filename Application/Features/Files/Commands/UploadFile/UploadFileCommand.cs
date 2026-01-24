using Application.Features.Files.Dtos;
using Domain.Enums;

namespace Application.Features.Files.Commands.UploadFile
{
    public sealed record UploadFileCommand(string Name, long Size, FileType Type, string Folder, Dictionary<string, string>? Metadata, ProcessingDto? Processing)
        : IRequest<UploadFileDto>;
}
