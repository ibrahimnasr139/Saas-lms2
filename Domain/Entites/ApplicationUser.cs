using Domain.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entites
{
    public sealed class ApplicationUser : IdentityUser, IAuditable
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? LastActiveTenantSubDomain { get; set; }
        public bool HasOnboarded { get; set; }
        public string? ProfilePicture { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
