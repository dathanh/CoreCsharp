using Framework.DomainModel.Entities;
using Framework.Repositories;
using System.Collections.Generic;

namespace Repositories.Interfaces
{
    public interface IUserRoleFunctionRepository : IEntityFrameworkRepository<UserRoleFunction>, IQueryableRepository<UserRoleFunction>
    {
        List<UserRoleFunction> LoadUserSecurityRoleFunction(int userRoleId, int documentTypeId);
    }
}
