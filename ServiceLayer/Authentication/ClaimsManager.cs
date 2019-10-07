using Framework.DomainModel.Entities;
using Framework.Utility;
using Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace ServiceLayer.Authentication
{
    public class ClaimsManager : IClaimsManager
    {
        public ClaimsManager(IUserRepository userRepository)
        {
            UserRepository = userRepository;
        }

        public IUserRepository UserRepository { get; set; }

        public IEnumerable<Claim> CreateClaims(string username, string password, string appRole = "")
        {
            // Construct the claims resulting from entering a username / password:
            // 1) Provider = Username / Password
            var providerClaim = new Claim(ClaimsDeclaration.AuthenticationTypeClaimType,
                ClaimsDeclaration.AuthenticationTypeClaimUsernamePassword);

            // 2) Username Claim
            var nameClaim = new Claim(ClaimsDeclaration.NameClaimType, username);

            // 3) Password Claim
            var passwordClaim = new Claim(ClaimsDeclaration.PasswordClaim, password);

            // 4) AppRole Claim
            var appRoleClaim = new Claim(ClaimsDeclaration.AppRoleClaim, appRole);

            var claims = new List<Claim> { nameClaim, providerClaim, passwordClaim, appRoleClaim };

            return claims;
        }

        public User ValidateExpressProjectLogin(List<Claim> claimset)
        {
            var user = new User();
            // determine, wether we want to evaluate username/password
            if (claimset.Any(x => ((x.Type == ClaimsDeclaration.AuthenticationTypeClaimType) &&
                                   (x.Value ==
                                    ClaimsDeclaration.AuthenticationTypeClaimUsernamePassword))))
            {
                var passwordClaim = from claim in claimset
                                    where
                                        claim.Type == ClaimsDeclaration.PasswordClaim
                                    select claim.Value;

                var nameClaim = (from claim in claimset
                                 where claim.Type == ClaimsDeclaration.NameClaimType
                                 select claim.Value).SingleOrDefault();

                var appRoleClaim = from claim in claimset
                                   where
                                       claim.Type == ClaimsDeclaration.AppRoleClaim
                                   select claim.Value;

                user.UserName = nameClaim;
                var userWithUserName = UserRepository.FirstOrDefault(o => o.UserName == nameClaim);

                // when validated successfully, remove password claims from the claimset for security
                var userLogin = UserRepository.GetUserByUserNameAndPass(nameClaim, passwordClaim.FirstOrDefault(), appRoleClaim.FirstOrDefault());
                if (userLogin == null)
                {
                    // Get userWithUsername
                    if (userWithUserName != null)
                    {
                        UserRepository.Commit();
                    }
                    return user;
                }

                UserRepository.Commit();
                var passwordClaims =
                    claimset.Where(x => x.Type == ClaimsDeclaration.PasswordClaim).ToList();

                passwordClaims.ForEach(x => claimset.Remove(x)); // remove password for safely.
                user = userLogin;
                user.IsWebProjectUser = true;
                return user;
            }

            return user;
        }
    }
}
