using Framework.Utility;

namespace Framework.DomainModel.ValueObject
{
    public class SeriesGridVo : ReadOnlyGridVo
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int OrderNumber { get; set; }
        public string Background { get; set; }
        public string BackgroundFormat
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(Background))
                {
                    return ImageHelper.GetImageUrlCdn() + Background;
                }
                return "";
            }
        }
    }
}
