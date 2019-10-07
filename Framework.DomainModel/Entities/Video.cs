using Framework.DataAnnotations;
using System.Collections.Generic;

namespace Framework.DomainModel.Entities
{
    public class Video : Entity
    {
        [LocalizeRequired(FieldName = "Name")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string UrlLink { get; set; }
        public int? CategoryId { get; set; }
        public int? SeriesId { get; set; }
        public decimal ViewNumber { get; set; }
        public bool? IsTrending { get; set; }
        public bool? IsPopular { get; set; }
        public string Avatar { get; set; }
        public int Duration { get; set; }
        public string TimeDuration { get; set; }
        public bool IsActive { get; set; }
        public virtual Category Category { get; set; }
        public virtual Series Series { get; set; }
        public virtual ICollection<VideoLanguage> VideoLanguages { get; set; } = new List<VideoLanguage>();
        public virtual ICollection<VideoPlayList> VideoPlayLists { get; set; } = new List<VideoPlayList>();
        public virtual ICollection<CustomerVideoWatched> CustomerVideoWatcheds { get; set; } = new List<CustomerVideoWatched>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public virtual ICollection<LikeVideo> LikeVideos { get; set; } = new List<LikeVideo>();
        public virtual ICollection<Banner> Banners { get; set; } = new List<Banner>();

    }
}