using Framework.Utility;

namespace Framework.DomainModel.ValueObject
{
    public class VideoGridVo : ReadOnlyGridVo
    {
        public string Name { get; set; }
        public bool? IsTrending { get; set; }
        public bool? IsPopular { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Avatar { get; set; }
        public string TimeDuration { get; set; }
        public bool IsActive { get; set; }
        public string FormatAvatar
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(Avatar))
                {
                    return ImageHelper.GetImageUrlCdn() + Avatar;
                }
                return "";
            }
        }

        public string ParentCategory { get; set; }
        public string Series { get; set; }
    }
}