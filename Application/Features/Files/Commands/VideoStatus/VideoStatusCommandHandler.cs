
using Application.Contracts.Repositories;
using Domain.Enums;

namespace Application.Features.Files.Commands.VideoStatus
{
    internal sealed class VideoStatusCommandHandler : IRequestHandler<VideoStatusCommand, Unit>
    {
        private readonly IFileRepository _fileRepository;
        public VideoStatusCommandHandler(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }
        public async Task<Unit> Handle(VideoStatusCommand request, CancellationToken cancellationToken)
        {
            var file = await _fileRepository.GetFileByIdAsync(request.Id, cancellationToken);
            if (file == null)
                return Unit.Value;

            if (request.Status == FileStatus.Processing.ToString())
                file.Status = FileStatus.Processing;
            else
                await _fileRepository.DeleteFileAsync(file, cancellationToken);

            await _fileRepository.SaveAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
