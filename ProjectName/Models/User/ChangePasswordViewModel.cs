using Framework.DataAnnotations;

namespace ProjectName.Models.User
{
    public class ChangePasswordViewModel
    {
        public int Id { get; set; }
        [LocalizeRequired]
        public string Username { get; set; }
        [LocalizeRequired]
        public string NewPassword { get; set; }
        [LocalizeRequired]
        public string ConfirmNewPassword { get; set; }
    }
}