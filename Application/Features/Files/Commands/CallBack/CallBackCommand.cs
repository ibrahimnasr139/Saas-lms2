using Domain.Enums;

namespace Application.Features.Files.Commands.CallBack
{
    public record CallBackCommand(FileStatus Status, string FileId) : IRequest<Unit> { }
}
