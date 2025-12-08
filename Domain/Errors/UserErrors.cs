using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Domain.Errors
{
    public static class UserErrors
    {
        public static Error UserAlreadyExists =>
            new Error("User.AlreadyExists", "البريد الإلكتروني موجود بالفعل", HttpStatusCode.Conflict);

        public static Error InvalidOtpCode =>
            new Error("User.InvalidOtpCode", "كود التحقق غير صحيح", HttpStatusCode.BadRequest);

        public static Error InvalidCredentials =>
            new Error("User.InvalidCredentials", "بيانات الاعتماد غير صحيحة", HttpStatusCode.Unauthorized);

        public static Error EmailNotConfirmed =>
            new Error("User.EmailNotConfirmed", "البريد الإلكتروني لم يتم تأكيده", HttpStatusCode.Unauthorized);

        public static Error EmailNotFound =>
            new Error("User.EmailNotFound", "البريد الإلكتروني غير موجود", HttpStatusCode.NotFound);

        public static Error Unauthorized =>
            new Error("User.Unauthorized", "غير مصرح", HttpStatusCode.Unauthorized);
    }
}
