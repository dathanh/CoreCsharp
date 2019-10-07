using Framework.DataAnnotations;
using System.Collections.Generic;

namespace Framework.DomainModel.Entities
{
    public class Banner : Entity
    {
        [LocalizeRequired(FieldName = "Name")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Background { get; set; }
        public string UrlLink { get; set; }
        public int OrderNumber { get; set; }
        public bool? IsActive { get; set; }
        public int Type { get; set; }
        public string TimeDuration { get; set; }
        public string VideoFile { get; set; }
        public string VideoOriginName { get; set; }
        public int? VideoId { get; set; }
        public bool? IsHideDescription { get; set; }
        public virtual ICollection<BannerLanguage> BannerLanguages { get; set; } = new List<BannerLanguage>();
        public virtual ICollection<VideoPlayList> VideoPlayLists { get; set; } = new List<VideoPlayList>();
        public virtual Video Video { get; set; }
    }
}
