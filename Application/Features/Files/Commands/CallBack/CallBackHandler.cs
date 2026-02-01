
using Application.Contracts.Repositories;
using Domain.Enums;

namespace Application.Features.Files.Commands.CallBack
{
    internal class CallBackHandler : IRequestHandler<CallBackCommand, Unit>
    {
        private readonly IFileRepository _fileRepository;

        public CallBackHandler(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }
        public async Task<Unit> Handle(CallBackCommand request, CancellationToken cancellationToken)
        {
            var file = await _fileRepository.GetFileByIdAsync(request.FileId, cancellationToken);
            if (file != null)
            {
                if (request.Status == FileStatus.Success)
                    file.Status = FileStatus.Success;
                else
                    await _fileRepository.DeleteFileAsync(file, cancellationToken);

                await _fileRepository.SaveAsync(cancellationToken);
            }
            return Unit.Value;
        }
    }
}
