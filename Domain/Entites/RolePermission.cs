namespace Domain.Entites
{
    public sealed class RolePermission
    {
        public int TenantRoleId { get; set; }
        public TenantRole TenantRole { get; set; } = null!;
        public string PermissionId { get; set; } = string.Empty;
        public Permission Permission { get; set; } = null!;
    }
}
