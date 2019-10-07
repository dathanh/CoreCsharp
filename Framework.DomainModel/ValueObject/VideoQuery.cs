namespace Framework.DomainModel.ValueObject
{
    public class VideoQuery : QueryInfo
    {
        public int? ParentCategoryId { get; set; }
        public int? CategoryId { get; set; }
        public int? SeriesId { get; set; }
    }
}