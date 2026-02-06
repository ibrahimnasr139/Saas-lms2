using Application.Constants;
using Application.Contracts.Files;
using Application.Contracts.Repositories;
using Application.Features.Files.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Application.Features.Files.Commands.CreateUpload
{
    internal class CreateUploadCommandHandler : IRequestHandler<CreateUploadCommand, OneOf<CreateUploadDto, Error>>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPlanRepository _planRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly ITenantRepository _tenantRepository;
        private readonly IFileService _fileService;
        private readonly ILogger<CreateUploadCommandHandler> _logger;
        private const int OverFlowSize = 20;

        public CreateUploadCommandHandler(IHttpContextAccessor httpContextAccessor, IPlanRepository planRepository,
            ISubscriptionRepository subscriptionRepository, ITenantRepository tenantRepository, IFileService fileService,
            ILogger<CreateUploadCommandHandler> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _planRepository = planRepository;
            _subscriptionRepository = subscriptionRepository;
            _tenantRepository = tenantRepository;
            _fileService = fileService;
            _logger = logger;
        }

        public async Task<OneOf<CreateUploadDto, Error>> Handle(CreateUploadCommand request, CancellationToken cancellationToken)
        {

            _logger.LogWarning( "CreateUpload started | Title: {Title}, Size: {Size}",request.Title, request.Size);

            var subDomain = _httpContextAccessor.HttpContext?.Request.Cookies[AuthConstants.SubDomain];
            _logger.LogWarning(
                "SubDomain cookie value: {SubDomain}",
                string.IsNullOrEmpty(subDomain) ? "NULL / EMPTY" : subDomain);


            var tenantId = await _tenantRepository.GetTenantIdAsync(subDomain!, cancellationToken);
            _logger.LogWarning("Resolved TenantId: {TenantId}", tenantId);


            var planPricingId = await _subscriptionRepository.GetPlanPricingIdAsync(tenantId, cancellationToken);
            _logger.LogWarning("PlanPricingId: {PlanPricingId}", planPricingId);


            var planId = await _planRepository.GetPlanIdAsync(planPricingId, cancellationToken);
            _logger.LogWarning("PlanId: {PlanId}", planId);


            var featureId = await _planRepository.GetVideoStorageFeatureIdAsync(cancellationToken);
            _logger.LogWarning("VideoStorage FeatureId: {FeatureId}", featureId);


            var planFeatureId = await _planRepository.GetPlanFeatureIdByFeatureIdAsync(planId, featureId, cancellationToken);
            _logger.LogWarning("PlanFeatureId: {PlanFeatureId}", planFeatureId);



            var limitValue = await _planRepository.GetVideoStorageLimitAsync(cancellationToken);
            _logger.LogWarning("VideoStorage Limit: {LimitValue}", limitValue);


            var used = await _tenantRepository.GetPlanFeatureUsageAsync(planFeatureId, cancellationToken);
            _logger.LogWarning("Current Used Storage: {Used}", used);


            var totalAfterUpload = (request.Size + used) - OverFlowSize;
            _logger.LogWarning(
               "TotalAfterUpload: {TotalAfterUpload} (Used: {Used} + Size: {Size} - Overflow: {Overflow})",
               totalAfterUpload, used, request.Size, OverFlowSize);

            if (totalAfterUpload > limitValue)
                return FileError.UploadFailed;

            var credentials = await _fileService.CreateUploadCredentialsAsync(request.Title, request.Size, cancellationToken);

            if (credentials == null)
            {
                _logger.LogError("Failed to create upload credentials");
                return FileError.UploadFailed;
            }

            return credentials == null ? FileError.UploadFailed : credentials;
        }
    }
}
