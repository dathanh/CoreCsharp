using Framework.DomainModel.ValueObject.Auth.Interfaces;
using Framework.DomainModel.ValueObject.Auth.UseCaseResponses;

namespace Framework.DomainModel.ValueObject.Auth.UseCaseRequests
{
    public class LoginRequest : IUseCaseRequest<LoginResponse>
    {
        public string UserName { get; }
        public string Password { get; }

        public LoginRequest(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }
    }
}
