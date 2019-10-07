using Framework.Utility;

namespace Framework.DomainModel.ValueObject
{
    public class BannerItem : DtoBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Background { get; set; }
        public string UrlLink { get; set; }
        public string TimeDuration { get; set; }
        public string VideoFile { get; set; }
        public int? VideoId { get; set; }
        public bool? IsHideDescription { get; set; }
        public string NameVideo { get; set; }
        public string NameUrl => NameVideo.GetUrlViaName();
    }
}

