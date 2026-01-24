namespace Domain.Entites
{
    public sealed class Subject
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; } = null!;
        public string Value { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
    }
}
