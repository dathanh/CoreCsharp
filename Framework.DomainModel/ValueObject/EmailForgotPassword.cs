
namespace Framework.DomainModel.ValueObject
{
    public class EmailForgotPassword : DtoBase
    {
        public string Email { get; set; }
        public int? LanguageId { get; set; }
    }
}

