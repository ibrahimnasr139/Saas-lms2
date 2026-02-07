namespace Application.Features.Roles.Dtos
{
    public sealed class TenantRolesDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool HasFullAccess { get; set; }
        public int PermissionsCount { get; set; }
        public bool IsSystemRole { get; set; }
        public int MembersCount { get; set; }
        public List<string> EnabledPermissions { get; set; } = [];
    }
}
