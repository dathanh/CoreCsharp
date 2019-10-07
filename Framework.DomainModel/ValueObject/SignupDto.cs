namespace Framework.DomainModel.ValueObject
{
    public class SignupDto : DtoBase
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public int? LanguageId { get; set; }
        public string ConfirmPass { get; set; }
        public bool IsCheckCondition { get; set; }
    }
}