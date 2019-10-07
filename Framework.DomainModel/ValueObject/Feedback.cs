namespace Framework.DomainModel.ValueObject
{
    public class Feedback
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public int? LanguageId { get; set; }
    }
}
