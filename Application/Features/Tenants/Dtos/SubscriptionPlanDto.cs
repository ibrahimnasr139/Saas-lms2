namespace Application.Features.Tenants.Dtos
{
    public sealed class SubscriptionPlanDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
