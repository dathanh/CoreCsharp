using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.ValueObject;
using Framework.Utility;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Repositories.Interfaces;
using ServiceLayer.Interfaces.Common;
using System.Collections.Generic;
using System.Linq;

namespace ServiceLayer.Authorization
{
    public class OperationAuthorization : IOperationAuthorization
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRoleFunctionRepository _userRoleFunctionRepository;
        private readonly IMenuExtractData _menuExtractData;
        public OperationAuthorization(IHttpContextAccessor httpContextAccessor,
                                      IUserRoleFunctionRepository userRoleFunctionRepository,
                                      IMenuExtractData menuExtractData)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRoleFunctionRepository = userRoleFunctionRepository;
            _menuExtractData = menuExtractData;
        }

        public bool VerifyAccess(DocumentTypeKey documentType, OperationAction action,
                                       out List<UserRoleFunction> permissionOfThisView)
        {
            UserDto principal = null;
            var userDataClaim = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(o => o.Type == ClaimsDeclaration.UserDataClaim);
            if (userDataClaim != null)
            {
                var userDataJson = userDataClaim.Value;
                principal = JsonConvert.DeserializeObject<UserDto>(userDataJson);

            }

            var hasPermission = false;
            permissionOfThisView = null;

            if (principal != null)
            {
                var userRoleId = principal.UserRoleId.GetValueOrDefault();

                var userGroupRights = _menuExtractData.LoadUserSecurityRoleFunction(userRoleId,
                    (int)documentType);
                if (userGroupRights == null || userGroupRights.Count == 0)
                {
                    userGroupRights = _userRoleFunctionRepository.LoadUserSecurityRoleFunction(userRoleId, (int)documentType);
                }

                if (userGroupRights != null)
                {
                    hasPermission = userGroupRights.Any(r => r.SecurityOperationId == (int)action);
                    permissionOfThisView = userGroupRights.ToList();
                }

            }

            return hasPermission;
        }
    }
}