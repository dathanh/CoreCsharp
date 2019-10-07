using Framework.DomainModel.Entities;
using Framework.DomainModel.ValueObject;

namespace ServiceLayer.Interfaces
{
    public interface IUserService : IMasterFileService<User>
    {
        //Task<UserDto> GetUserByUserNameAndPass(string username, string password);
        bool HasPermission(int userId, int documentTypeId, int operationAction);
        bool SaveChangePassword(int id, string newPassword, string confirmPassword, string oldPassword);

        bool SaveChangePassword(int id, string newPassword, string confirmNewPassword);
        UserDto GetUserByUserNameAndPass(string username, string password);
    }
}