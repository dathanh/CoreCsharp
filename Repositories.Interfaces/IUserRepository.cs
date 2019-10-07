using Framework.DomainModel.Entities;
using Framework.Repositories;

namespace Repositories.Interfaces
{
    public interface IUserRepository : IEntityFrameworkRepository<User>, IQueryableRepository<User>
    {
        User GetUserByUserNameAndPass(string username, string password, string appRole = "");
        bool HasPermission(int userId, int documentTypeId, int operationAction);
        bool UpdatePassword(int id, string newPassword);
    }
}
