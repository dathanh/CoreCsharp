using Framework.DomainModel.ValueObject.Auth.Interfaces;
using Framework.DomainModel.ValueObject.Auth.UseCaseResponses;

namespace Framework.DomainModel.ValueObject.Auth.UseCaseRequests
{
    public class ExchangeRefreshTokenRequest : IUseCaseRequest<ExchangeRefreshTokenResponse>
    {
        public string AccessToken { get; }
        public string RefreshToken { get; }
        public string SigningKey { get; }

        public ExchangeRefreshTokenRequest(string accessToken = "", string refreshToken = "", string signingKey = "")
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            SigningKey = signingKey;
        }
    }
}
