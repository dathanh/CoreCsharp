using Framework.DomainModel.Entities;
using System.Collections.Generic;
using System.Security.Claims;

namespace ServiceLayer.Authentication
{
    public interface IClaimsManager
    {
        IEnumerable<Claim> CreateClaims(string username, string password, string appRole = "");
        User ValidateExpressProjectLogin(List<Claim> claimset);
    }
}
