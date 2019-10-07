namespace Framework.DomainModel.ValueObject
{
    public class SigninWithFbDto : DtoBase
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AvatarUrl { get; set; }
        public int? LanguageId { get; set; }
        public string FbId { get; set; }
        public string AccessToken { get; set; }
    }
}
