using ProjectName.Models.Base;
using System.Collections.Generic;

namespace ProjectName.Models.Banner
{
    public class DashboardBannerShareViewModel : DashboardSharedViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Background { get; set; }
        public string UrlLink { get; set; }
        public int? VideoId { get; set; }
        public string VideoName { get; set; }
        public List<VideoItemUpload> VideoDetails { get; set; } = new List<VideoItemUpload>();
        public int? OrderNumber { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsHideDescription { get; set; }
        public int? Type { get; set; }
        public string TimeDuration { get; set; }
        public string NameInVietnamese { get; set; }
        public string DescriptionInVietnamese { get; set; }
        public string BackgroundInVietnamese { get; set; }
        public string TimeDurationInVietnamese { get; set; }
    }
    public class VideoItemUpload
    {
        public string VideoLink { get; set; }
        public string FileNameOriginal { get; set; }
        public string FilePath { get; set; }
    }
}