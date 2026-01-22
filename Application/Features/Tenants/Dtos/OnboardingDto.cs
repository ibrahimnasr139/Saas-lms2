using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Tenants.Dtos
{
    public sealed class OnboardingDto
    {
        public string Subdomain { get; set; } = string.Empty;
        public string Message { get; set; } = "تم انشاء منصتك بنجاح";
    }
}
