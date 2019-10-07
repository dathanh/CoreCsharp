namespace Framework.DomainModel.ValueObject
{
    public class CommentDto : DtoBase
    {
        public string Message { get; set; }
        public int CustomerId { get; set; }
        public int VideoId { get; set; }
        public int? ParentId { get; set; }
    }
}