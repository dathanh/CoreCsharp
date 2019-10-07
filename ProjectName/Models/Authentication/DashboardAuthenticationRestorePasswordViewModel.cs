using Framework.DataAnnotations;
using Framework.Service.Translation;
using ProjectName.Models.Base;

namespace ProjectName.Models.Authentication
{
    public class DashboardAuthenticationRestorePasswordViewModel : ViewModelBase
    {
        public override string PageTitle => SystemMessageLookup.GetMessage("RestorePasswordPageTitle");
        [LocalizeRequired]
        [LocalizeEmailAddress]
        public string Email { get; set; }
    }
}