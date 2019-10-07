using Framework.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Framework.DomainModel.Interfaces
{
    public class WebProjectIdentity : ClaimsIdentity, IWebProjectIdentity
    {
        private readonly List<Claim> _claims;

        #region Constructors

        /// <summary>
        ///     Creates an unauthenticated Identity.
        /// </summary>
        /// <remarks>
        ///     An ExpressProjectIdentity created with this constructor represents a user before
        ///     he is authenicated.
        /// </remarks>
        public WebProjectIdentity()
        {
        }

        /// <summary>
        /// Creates an Advantage identity from a claims
        /// </summary>
        /// <param name="claims">Set of claims used to initialise the identity.</param>
        public WebProjectIdentity(IEnumerable<Claim> claims)
        {
            var nameClaim = from claim in claims
                            where claim.Type == ClaimsDeclaration.NameClaimType
                            select claim.Value;

            Name = nameClaim.Any() ? nameClaim.First() : string.Empty;

            // set authentication type claim
            var authTypeClaim = claims.FirstOrDefault(x => x.Type == ClaimsDeclaration.AuthenticationTypeClaimType);

            AuthenticationType = authTypeClaim == null ? null : authTypeClaim.Value;
            IsAuthenticated = nameClaim.Count() == 1;
        }


        /// <summary>
        ///     Creates an AdvantageIdentity for a user specified by his name and UserId that
        ///     has been authenticated by a specific authentication method.
        /// </summary>
        public WebProjectIdentity(string name, int employeeId, string authenticationType)
        {
            Name = name;
            UserIdentityId = employeeId;
            IsAuthenticated = true;
            AuthenticationType = authenticationType;

            // add authentication type and name to claims
            _claims = new List<Claim>
            {
                new Claim(ClaimsDeclaration.NameClaimType, name),
                new Claim(ClaimsDeclaration.AuthenticationTypeClaimType, authenticationType)
            };
        }

        public WebProjectIdentity(string name, int userIdentityId, List<Claim> claims)
        {
            Name = name;
            UserIdentityId = userIdentityId;
            IsAuthenticated = true;
            _claims = claims;
        }

        #endregion

        public new IEnumerable<Claim> Claims => _claims;

        /// <summary>
        ///     Gets and sets the user's e-mail address, if defined.
        /// </summary>
        public string EmailAddress { get; set; }

        #region IIdentity Members

        /// <summary>
        ///     Gets a value that indicates whether the user has been authenticated.
        /// </summary>
        public new bool IsAuthenticated { get; set; }

        /// <summary>
        ///     Gets the name of the current user.
        /// </summary>
        public new string Name { get; set; }

        /// <summary>
        ///     Gets the type of authentication used.
        /// </summary>
        public new string AuthenticationType { get; set; }

        #endregion

        #region Fixed Indentities

        /// <summary>
        ///     Identity of a non-authentication person. This serves
        ///     as a placeholder when an Identity is required that is explicitly
        ///     not an authenticated user.
        /// </summary>
        public static WebProjectIdentity Unauthenticated => new WebProjectIdentity();

        #endregion

        public int UserIdentityId { get; set; }
    }
}
