namespace Framework.DomainModel.ValueObject
{
    public class CustomerChangePasswordDto : DtoBase
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string RetypePassword { get; set; }
        public int? LanguageId { get; set; }
    }
}