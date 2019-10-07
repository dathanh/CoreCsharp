using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Security.Principal;

namespace Framework.Utility
{
    public class WebProjectHttpContext : IWebProjectHttpContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public WebProjectHttpContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public HttpContext Context => _httpContextAccessor.HttpContext;

        public HttpRequest Request => _httpContextAccessor.HttpContext.Request;

        public HttpResponse Response => _httpContextAccessor.HttpContext.Response;

        public IPrincipal User
        {
            get => _httpContextAccessor.HttpContext.User;
            set => _httpContextAccessor.HttpContext.User = (ClaimsPrincipal)value;
        }
    }
}
