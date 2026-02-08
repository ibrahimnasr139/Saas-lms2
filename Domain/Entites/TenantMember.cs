namespace Domain.Entites
{
    public sealed class TenantMember
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; } = null!;
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;
        public int TenantRoleId { get; set; }
        public TenantRole TenantRole { get; set; } = null!;
        public int? InvitedById { get; set; }
        public TenantMember? InvitedBy { get; set; }
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
        public int ExperienceYears { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public string? Bio { get; set; }
        public string? DisplayName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public ICollection<TenantInvite> TenantInvites { get; set; } = [];
    }
}
