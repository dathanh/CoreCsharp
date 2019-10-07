
using Framework.Utility;

namespace Framework.DomainModel.ValueObject
{
    public class VideoItem : DtoBase
    {
        public string Name { get; set; }
        public string UrlLink { get; set; }
        public string Avatar { get; set; }
        public string TimeDuration { get; set; }
        public string Description { get; set; }
        public string NameUrl => Name.GetUrlViaName();
    }
}
