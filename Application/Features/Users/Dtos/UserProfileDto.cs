using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Users.Dtos
{
    public sealed class UserProfileDto
    {
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string LastActiveTenantSubDomain { get; set; } = string.Empty;
        public bool HasOnboarded { get; set; }
        public bool IsSubscribed { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}
