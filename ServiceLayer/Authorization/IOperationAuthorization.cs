using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.ValueObject;
using System.Collections.Generic;

namespace ServiceLayer.Authorization
{
    public interface IOperationAuthorization
    {
        bool VerifyAccess(DocumentTypeKey documentType, OperationAction action, out List<UserRoleFunction> permissionOfThisView);

    }
}
