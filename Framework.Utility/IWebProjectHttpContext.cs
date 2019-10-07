using Microsoft.AspNetCore.Http;
using System.Security.Principal;

namespace Framework.Utility
{
    public interface IWebProjectHttpContext
    {
        HttpContext Context { get; }
        HttpRequest Request { get; }
        HttpResponse Response { get; }
        IPrincipal User { get; set; }
    }
}
