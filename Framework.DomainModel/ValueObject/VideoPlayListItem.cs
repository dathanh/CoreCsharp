
namespace Framework.DomainModel.ValueObject
{
    public class VideoPlayListItem : DtoBase
    {
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string UrlLink { get; set; }
        public string Description { get; set; }
    }

    public class VideoPlayListItemDetail : DtoBase
    {
        public bool IsVideo => VideoId.GetValueOrDefault() != 0;
        public string Name { get; set; }
        public string Avatar { get;set; }
        public string UrlLink { get; set; }
        public string Description { get; set; }
        public int? VideoId { get; set; }
        public int? CurrentDuration { get; set; }
    }
}
