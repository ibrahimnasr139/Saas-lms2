using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Domain.Errors
{
    public static class UserErrors
    {
        public static Error UserAlreadyExists =>
            new Error("User.AlreadyExists", "Email already exists", HttpStatusCode.Conflict);
        public static Error InvalidOtpCode =>
            new Error("User.InvalidOtpCode", "Invalid OTP code", HttpStatusCode.BadRequest);
        public static Error InvalidCredentials =>
            new Error("User.InvalidCredentials", "Invalid credentials", HttpStatusCode.Unauthorized);
        public static Error EmailNotConfirmed =>
            new Error("User.EmailNotConfirmed", "Email not confirmed", HttpStatusCode.Unauthorized);
        public static Error EmailNotFound =>
            new Error("User.EmailNotFound", "Email does not exist", HttpStatusCode.NotFound);

    }
}
