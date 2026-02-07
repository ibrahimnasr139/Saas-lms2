namespace Application.Features.Tenants.Dtos
{
    public sealed class TenantUsageDto
    {
        public SubscriptionDto Subscription { get; set; } = new SubscriptionDto();
        public List<UsageDto> Usage { get; set; } = [];
    }
}
