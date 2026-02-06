using Application.Common;
using Application.Constants;
using Application.Contracts.Files;
using Application.Contracts.Repositories;
using Application.Features.Files.Dtos;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Files.Commands.CreateUpload
{
    internal class CreateUploadCommandHandler : IRequestHandler<CreateUploadCommand, OneOf<CreateUploadDto, Error>>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPlanRepository _planRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly ITenantRepository _tenantRepository;
        private readonly IFileService _fileService;
        private readonly IFileRepository _fileRepository;
        private readonly ICurrentUserId _currentUserId;

        private const int OverFlowSizeMB = 20;

        public CreateUploadCommandHandler(IHttpContextAccessor httpContextAccessor, IPlanRepository planRepository,
            ISubscriptionRepository subscriptionRepository, ITenantRepository tenantRepository, IFileService fileService,
            IFileRepository fileRepository, ICurrentUserId currentUserId)
        {
            _httpContextAccessor = httpContextAccessor;
            _planRepository = planRepository;
            _subscriptionRepository = subscriptionRepository;
            _tenantRepository = tenantRepository;
            _fileService = fileService;
            _fileRepository = fileRepository;
            _currentUserId = currentUserId;
        }

        public async Task<OneOf<CreateUploadDto, Error>> Handle(CreateUploadCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserId.GetUserId();
            var subDomain = _httpContextAccessor.HttpContext?.Request.Cookies[AuthConstants.SubDomain];
            var tenantId = await _tenantRepository.GetTenantIdAsync(subDomain!, cancellationToken);
            var planPricingId = await _subscriptionRepository.GetPlanPricingIdAsync(tenantId, cancellationToken);
            var planId = await _planRepository.GetPlanIdAsync(planPricingId, cancellationToken);
            var featureId = await _planRepository.GetVideoStorageFeatureIdAsync(cancellationToken);
            var planFeatureId = await _planRepository.GetPlanFeatureIdByFeatureIdAsync(planId, featureId, cancellationToken);

            var limitValueGB = await _planRepository.GetVideoStorageLimitAsync(cancellationToken);
            var limitMB = limitValueGB * 1024;

            var usedBytes = await _tenantRepository.GetPlanFeatureUsageAsync(planFeatureId, cancellationToken);
            var usedMB = Math.Max(0, usedBytes / (1024 * 1024));

            var requestMB = request.Size / (1024 * 1024);

            var totalAfterUpload = usedMB + requestMB - OverFlowSizeMB;

            if (totalAfterUpload > limitMB)
                return FileError.UploadFailed;

            var credentials = await _fileService.CreateUploadCredentialsAsync(request.Title, request.Size, cancellationToken);
            if (credentials == null)
                return FileError.UploadFailed;

            var fileEntity = await CreateFileEntity(credentials.VideoId, request.Title, request.Size, credentials.EmbedUrl, tenantId, userId);
            await _fileRepository.CreateAsync(fileEntity, cancellationToken);
            await _fileRepository.SaveAsync(cancellationToken);
            return credentials;
        }

        private async Task<Domain.Entites.File> CreateFileEntity(string videoId, string title, int size, string EmbedUrl, int tenantId, string userId)
        {
            return new Domain.Entites.File
            {
                Id = videoId,
                Name = title,
                Size = size,
                Type = Domain.Enums.FileType.Video,
                Status = Domain.Enums.FileStatus.Pending,
                Url = EmbedUrl,
                TenantId = tenantId,
                UploadedById = userId
            };
        }
    }
}
