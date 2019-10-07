using ProjectName.Models.Base;

namespace ProjectName.Models.Video
{
    public class DashboardVideoShareViewModel : DashboardSharedViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string NameInVietnamese { get; set; }
        public string DescriptionInVietnamese { get; set; }
        public string AvatarInVietnamese { get; set; }
        public string UrlLink { get; set; }
        public int CategoryId { get; set; }
        public int? SeriesId { get; set; }
        public bool? IsTrending { get; set; }
        public bool? IsPopular { get; set; }
        public string Avatar { get; set; }
        public string TimeDuration { get; set; }
        public string TimeDurationInVietnamese { get; set; }
        public bool? IsActive { get; set; }

        public string ParentCategoryName { get; set; }
        public int? ParentCategoryId { get; set; }
        public string CategoryName { get; set; }
        public string SeriesName { get; set; }
    }
}