using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.ValueObject;
using Framework.Exceptions;
using Framework.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using ServiceLayer.Authorization;
using System.Linq;
using System.Threading.Tasks;


namespace ProjectName.Attributes
{
    public class AuthorizeActionFilter : IAsyncActionFilter
    {
        private readonly DocumentTypeKey _documentTypeKey;
        private readonly OperationAction _operationAction;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOperationAuthorization _operationAuthorization;
        public AuthorizeActionFilter(DocumentTypeKey documentTypeKey, OperationAction operationAction,
            IHttpContextAccessor httpContextAccessor, IOperationAuthorization operationAuthorization)
        {
            _documentTypeKey = documentTypeKey;
            _operationAction = operationAction;
            _httpContextAccessor = httpContextAccessor;
            _operationAuthorization = operationAuthorization;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            UserDto currentUser = null;
            var userDataClaim = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(o => o.Type == ClaimsDeclaration.UserDataClaim);
            if (userDataClaim != null)
            {
                var userDataJson = userDataClaim.Value;
                currentUser = JsonConvert.DeserializeObject<UserDto>(userDataJson);
            }

            if (currentUser == null || currentUser.Id == 0)
            {
                context.Result = new ForbidResult();
                return;
            }

            if (_documentTypeKey != DocumentTypeKey.None)
            {
                var permission = _operationAuthorization.VerifyAccess(_documentTypeKey, _operationAction, out _);
                var exception = new UnAuthorizedAccessException("NoPermission");
                if (!permission)
                {
                    throw exception;
                }
            }

            await next();
        }
    }
}