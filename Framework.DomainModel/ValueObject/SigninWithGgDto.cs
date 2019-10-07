namespace Framework.DomainModel.ValueObject
{
    public class SigninWithGgDto
    {
        public string CodeFromClient { get; set; }
        public string RedirectUrl { get; set; }
    }
}
