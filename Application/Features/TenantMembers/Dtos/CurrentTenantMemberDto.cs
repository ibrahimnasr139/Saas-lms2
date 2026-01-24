namespace Application.Features.TenantMembers.Dtos
{
    public sealed class CurrentTenantMemberDto
    {
        public string UserId { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } = string.Empty;
        public string? ProfilePicture { get; set; } = string.Empty;
        public int TenantMemberId { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public int ExperienceYears { get; set; }
        public string Bio { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool HasFullAccess { get; set; }
        public List<string>? permissions { get; set; } = [];
        public NotificationsDto Notifications { get; set; } = new();
    }
}
