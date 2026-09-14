using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid? UserId
        {
            get
            {
                var idString = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                               ?? _httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value;

                if (!string.IsNullOrEmpty(idString) && Guid.TryParse(idString, out var userId))
                {
                    return userId;
                }

                return null;
            }
        }
    }
}