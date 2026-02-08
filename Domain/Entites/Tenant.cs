using Domain.Abstractions;

namespace Domain.Entites
{
    public sealed class Tenant : IAuditable
    {
        public int Id { get; set; }
        public string PlatformName { get; set; } = string.Empty;
        public string SubDomain { get; set; } = string.Empty;
        public string? Logo { get; set; } = string.Empty;
        public string OwnerId { get; set; } = string.Empty;
        public ApplicationUser Owner { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public ICollection<Grade> Grades { get; set; } = [];
        public ICollection<TenantRole> TenantRoles { get; set; } = [];
        public ICollection<TenantMember> TenantMembers { get; set; } = [];
        public ICollection<TeachingLevel> TeachingLevels { get; set; } = [];
        public ICollection<Subject> Subjects { get; set; } = [];
        public ICollection<Subscription> Subscriptions { get; set; } = [];
        public ICollection<Course> Courses { get; set; } = [];
        public ICollection<TenantUsage> TenantUsages { get; set; } = [];
        public ICollection<TenantInvite> TenantInvites { get; set; } = [];
    }
}
