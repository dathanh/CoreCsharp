using Framework.DomainModel.Entities;
using Framework.DomainModel.ValueObject;
using System.Threading.Tasks;

namespace ServiceLayer.Interfaces
{
    public interface IAuthenticationService
    {
        Task<UserDto> GetCurrentUser();
        Task SignOut();
        Task<UserDto> SignIn(string userName, string password, bool rememberMe, string appRole = "");
        User RestorePassword(string email, out string passwordRandom);
    }
}