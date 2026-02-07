namespace Application.Features.Tenants.Dtos
{
    public sealed class SubscriptionDto
    {
        public int Id { get; set; }
        public SubscriptionPlanDto Plan { get; set; } = new SubscriptionPlanDto();
        public string Status { get; set; } = string.Empty;
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
    }
}
