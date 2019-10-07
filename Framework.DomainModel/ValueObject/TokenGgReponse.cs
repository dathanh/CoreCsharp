using Newtonsoft.Json;

namespace Framework.DomainModel.ValueObject
{
    public class TokenGgReponse : RefreshTokenGgResponse
    {
        [JsonProperty(PropertyName = "refresh_token")]
        public string RefreshToken { get; set; }
    }
}
