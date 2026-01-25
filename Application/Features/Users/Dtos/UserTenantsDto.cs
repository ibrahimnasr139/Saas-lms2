namespace Application.Features.Users.Dtos
{
    public sealed class UserTenantsDto
    {
        public int Id { get; set; }
        public string PlatformName { get; set; } = string.Empty;
        public string? Logo { get; set; } = string.Empty;
        public string Subdomain { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsCurrentTenant { get; set; }

    }
}
