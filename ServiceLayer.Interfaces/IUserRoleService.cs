using Framework.DomainModel.Entities;
using Framework.DomainModel.ValueObject;
using System.Collections.Generic;

namespace ServiceLayer.Interfaces
{
    public interface IUserRoleService : IMasterFileService<UserRole>
    {
        dynamic GetRoleFunction(int idRole);

        IEnumerable<int> GetAllDocumentTypeId();

        List<RoleDto> GetListRoles();
        List<UserRoleFunctionDto> GetListUserRoleFunction();
        List<DocumentTypeDto> GetListDocumentType();
        IList<UserRoleFunction> ProcessMappingFromUserRoleGrid(List<UserRoleFunctionGridVo> userRoleFunctionData);
    }
}