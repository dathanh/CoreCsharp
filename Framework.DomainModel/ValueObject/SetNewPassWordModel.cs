
namespace Framework.DomainModel.ValueObject
{
    public class SetNewPassWordModel
    {
        public string ForgotPasswordCode { get; set; }
        public int? LanguageId { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}

