using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Common
{
    internal class CurrentUserId : ICurrentUserId
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CurrentUserId(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string GetUserId()
        {
            return _httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)!;
        }
    }
}
