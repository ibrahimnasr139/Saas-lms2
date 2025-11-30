using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Domain.Errors
{
    public record Error(string Code, string Message, HttpStatusCode HttpStatusCode)
    {
        public static Error None => new Error(string.Empty, string.Empty, HttpStatusCode.OK);
    }
}
