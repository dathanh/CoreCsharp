using Framework.DataAnnotations;
using Framework.Service.Translation;
using ProjectName.Models.Base;

namespace ProjectName.Models.Authentication
{

    public class DashboardAuthenticationSignInViewModel : ViewModelBase
    {
        public override string PageTitle => SystemMessageLookup.GetMessage("SignInPageTitle");
        [LocalizeRequired]
        public string UserName { get; set; }
        [LocalizeRequired]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }

}