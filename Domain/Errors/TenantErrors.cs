using System.Net;

namespace Domain.Errors
{
    public static class TenantErrors
    {
        public static Error SubDomainAlreadyExists => new(
            "Tenant.SubDomainAlreadyExists",
            "هذا الدومين مستخدم بالفعل",
            HttpStatusCode.Conflict
        );
    }
}
