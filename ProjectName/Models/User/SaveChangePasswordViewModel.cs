namespace ProjectName.Models.User
{
    public class SaveChangePasswordViewModel
    {
        public string OldPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string Password { get; set; }
    }
}