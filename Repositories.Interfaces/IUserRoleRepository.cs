using Framework.DomainModel.Entities;
using Framework.DomainModel.ValueObject;
using Framework.Repositories;
using System.Collections.Generic;

namespace Repositories.Interfaces
{
    public interface IUserRoleRepository : IEntityFrameworkRepository<UserRole>, IQueryableRepository<UserRole>
    {
        dynamic GetRoleFunction(int idRole);

        IEnumerable<int> GetAllDocumentTypeId();
        List<DocumentType> GetAllDocumentType();
        List<RoleDto> GetListRoles();
        List<UserRoleFunctionDto> GetListUserRoleFunction();
        List<DocumentTypeDto> GetListDocumentType();
    }
}
