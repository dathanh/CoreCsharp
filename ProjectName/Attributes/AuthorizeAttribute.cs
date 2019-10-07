using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.ValueObject;
using Microsoft.AspNetCore.Mvc;

namespace ProjectName.Attributes
{
    public class AuthorizeAttribute : TypeFilterAttribute
    {
        public AuthorizeAttribute(DocumentTypeKey documentTypeKey, OperationAction operationAction)
        : base(typeof(AuthorizeActionFilter))
        {
            Arguments = new object[] { documentTypeKey, operationAction };
        }
    }
}