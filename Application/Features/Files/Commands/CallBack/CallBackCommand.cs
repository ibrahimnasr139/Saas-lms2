using Domain.Enums;

namespace Application.Features.Files.Commands.CallBack
{
    public record CallBackCommand(string JobId, FileStatus Status, string FileId) : IRequest<Unit> { }
}
