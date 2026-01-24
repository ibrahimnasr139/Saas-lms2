using Application.Common;
using Application.Contracts.Files;
using Application.Contracts.Repositories;
using Application.Features.Files.Dtos;

namespace Application.Features.Files.Commands.UploadFile
{
    internal class UploadFileCommandHandler : IRequestHandler<UploadFileCommand, UploadFileDto>
    {
        private readonly IFileService _fileService;
        private readonly IFileRepository _fileRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserId _currentUserId;
        public UploadFileCommandHandler(IFileService fileService, IFileRepository fileRepository, IMapper mapper, ICurrentUserId currentUserId)
        {
            _fileService = fileService;
            _fileRepository = fileRepository;
            _mapper = mapper;
            _currentUserId = currentUserId;
        }
        public async Task<UploadFileDto> Handle(UploadFileCommand request, CancellationToken cancellationToken)
        {
            var fileId = Guid.NewGuid().ToString();
            var path = _fileService.GetPath(fileId, request.Name, request.Folder);
            var uploadUrl = _fileService.CreateUploadUrl(path);
            var cdnUrl = _fileService.CreateCdnUrl(path);
            var userId = _currentUserId.GetUserId();
            var file = _mapper.Map<Domain.Entites.File>(request, opt =>
            {
                opt.AfterMap((src, dest) =>
                {
                    dest.Id = fileId;
                    dest.Url = cdnUrl;
                    dest.UploadedById = userId;
                });
            });
            await _fileRepository.CreateAsync(file, cancellationToken);
            await _fileRepository.SaveAsync(cancellationToken);
            return new UploadFileDto
            {
                UploadUrl = uploadUrl,
                CdnUrl = cdnUrl,
                Path = path,
                Metadata = request.Metadata,
            };
        }
    }
}
