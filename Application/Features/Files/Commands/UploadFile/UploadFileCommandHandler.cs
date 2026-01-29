using Application.Common;
using Application.Constants;
using Application.Contracts.Files;
using Application.Contracts.Repositories;
using Application.Features.Files.Dtos;
using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Files.Commands.UploadFile
{
    internal class UploadFileCommandHandler : IRequestHandler<UploadFileCommand, OneOf<UploadFileDto, Error>>
    {
        private readonly IFileService _fileService;
        private readonly IFileRepository _fileRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserId _currentUserId;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITenantRepository _tenantRepository;

        public UploadFileCommandHandler(IFileService fileService, IFileRepository fileRepository,
            IMapper mapper, ICurrentUserId currentUserId,IHttpContextAccessor httpContextAccessor, ITenantRepository tenantRepository)
        {
            _fileService = fileService;
            _fileRepository = fileRepository;
            _mapper = mapper;
            _currentUserId = currentUserId;
            _httpContextAccessor = httpContextAccessor;
            _tenantRepository = tenantRepository;
        }
        public async Task<OneOf<UploadFileDto, Error>> Handle(UploadFileCommand request, CancellationToken cancellationToken)
        {
            var fileType = _fileService.GetFileType(request.File.ContentType);
            var fileId = Guid.NewGuid().ToString();
            var fileName = request.Name ?? request.File.FileName;
            var path = _fileService.GetPath(fileId, fileName, request.Folder, request.File.FileName);
            var userId = _currentUserId.GetUserId();

            int? tenantId = null;
            var subdomain = _httpContextAccessor.HttpContext?.Request.Cookies[AuthConstants.SubDomain];
            if (subdomain != null)
                tenantId = await _tenantRepository.GetTenantIdAsync(subdomain!, cancellationToken);

            string? cdnUrl = await _fileService.UploadAsync(request.File, path, request.Folder);

            if (cdnUrl == null)
                return FileError.UploadFailed;

            var file = new Domain.Entites.File
            {
                Id = fileId,
                Name = request.Name != null
                    ? request.Name
                    : request.File.FileName,
                Size = request.File.Length,
                Type = fileType,
                Url = cdnUrl,
                UploadedById = userId,
                Status = request.Embedding?.Enabled == true
                    ? FileStatus.Processing
                    : FileStatus.Success,
                TenantId = tenantId
            };

            await _fileRepository.CreateAsync(file, cancellationToken);
            await _fileRepository.SaveAsync(cancellationToken);

            return new UploadFileDto
            {
                FileId = fileId,
                FileType = fileType.ToString(),
                Url = cdnUrl,
                OriginalName = request.Name != null
                    ? request.Name
                    : request.File.FileName,
                Size = request.File.Length
            };
        }
    }
}
