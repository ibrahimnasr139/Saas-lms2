using System.Net;

namespace Domain.Errors
{
    public static class TenantInviteError
    {
        public static Error UserAlreadyExists =>
            new Error("User.AlreadyExists", "هذا المستخدم عضو بالفعل في المنصة", HttpStatusCode.Conflict);

        public static Error InviteExpired =>
            new Error("Invite.Expired", "انتهت صلاحية الدعوة", HttpStatusCode.Gone);

        public static Error InviteNotFound =>
            new Error("Invite.NotFound", "الدعوة غير موجودة", HttpStatusCode.NotFound);
    }
}
