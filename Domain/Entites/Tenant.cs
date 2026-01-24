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
        public ICollection<Grade> Grades { get; set; } = new List<Grade>();
        public ICollection<TenantRole> TenantRoles { get; set; } = new List<TenantRole>();
        public ICollection<TenantMember> TenantMembers { get; set; } = new List<TenantMember>();
        public ICollection<TeachingLevel> TeachingLevels { get; set; } = new List<TeachingLevel>();
        public ICollection<Subject> Subjects { get; set; } = new List<Subject>();
        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    }
}
