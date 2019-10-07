
namespace Framework.Utility
{
    public static class ClaimsDeclaration
    {
        public const string NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
        public const string AuthenticationTypeClaimType =
            "http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod";

        public const string AuthenticationCookie = "http://schemas.microsoft.com/ws/2008/06/identity/claims/WorkCompPrincipal";
        public const string AuthenticationTypeClaimUsernamePassword = "http://schemas.microsoft.com/ws/2008/06/identity/claims/WorkCompCredential";
        public const string PasswordClaim = "http://schemas.microsoft.com/ws/2008/06/identity/claims/PasswordClaim";
        public const string IdRoleClaim = "http://schemas.microsoft.com/ws/2008/06/identity/claims/IdRoleClaim";
        public const string IdClaim = "http://schemas.microsoft.com/ws/2008/06/identity/claims/IdClaim";
        public const string UserDataClaim = "http://schemas.microsoft.com/ws/2008/06/identity/claims/UserDataClaim";
        public const string AppRoleClaim = "http://schemas.microsoft.com/ws/2008/06/identity/claims/AppRoleClaim";
    }
}
