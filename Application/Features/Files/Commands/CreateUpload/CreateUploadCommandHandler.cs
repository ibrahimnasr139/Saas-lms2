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

        private const int OverFlowSize = 20;

        public CreateUploadCommandHandler(IHttpContextAccessor httpContextAccessor, IPlanRepository planRepository,
            ISubscriptionRepository subscriptionRepository, ITenantRepository tenantRepository, IFileService fileService)
        {
            _httpContextAccessor = httpContextAccessor;
            _planRepository = planRepository;
            _subscriptionRepository = subscriptionRepository;
            _tenantRepository = tenantRepository;
            _fileService = fileService;
        }

        public async Task<OneOf<CreateUploadDto, Error>> Handle(CreateUploadCommand request, CancellationToken cancellationToken)
        {
            var subDomain = _httpContextAccessor.HttpContext?.Request.Cookies[AuthConstants.SubDomain];
            var tenantId = await _tenantRepository.GetTenantIdAsync(subDomain!, cancellationToken);
            var planPricingId = await _subscriptionRepository.GetPlanPricingIdAsync(tenantId, cancellationToken);
            var planId = await _planRepository.GetPlanIdAsync(planPricingId, cancellationToken);
            var featureId = await _planRepository.GetVideoStorageFeatureIdAsync(cancellationToken);
            var planFeatureId = await _planRepository.GetPlanFeatureIdByFeatureIdAsync(planId, featureId, cancellationToken);

            var limitValue = await _planRepository.GetVideoStorageLimitAsync(cancellationToken);
            var used = await _tenantRepository.GetPlanFeatureUsageAsync(planFeatureId, cancellationToken);
            var totalAfterUpload = (request.Size + used) - OverFlowSize;
            if (totalAfterUpload > limitValue)
                return FileError.UploadFailed;

            var credentials = await _fileService.CreateUploadCredentialsAsync(request.Title, request.Size, cancellationToken);

            return credentials == null ? FileError.UploadFailed : credentials;
        }
    }
}
