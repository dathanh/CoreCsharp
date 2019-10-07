using Framework.Utility;
using System;

using Framework.Utility;

namespace Framework.DomainModel.ValueObject
{
    public class VideoDetail : DtoBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedValue { get; set; }
        public string Created
        {
            get
            {
                if (CreatedValue.GetValueOrDefault() == DateTime.MinValue)
                {
                    return "";
                }
                return CreatedValue.GetValueOrDefault().ToString(ConstantValue.DateFormatVideoDetail);
            }
        }
        public string UrlLink { get; set; }
        public string Avatar { get; set; }
        public string TimeDuration { get; set; }
        public string Category { get; set; }
        public string CategoryNameUrl => Category.GetUrlViaName();
        public string SubCategory { get; set; }
        public string SubCategoryUrl => SubCategory.GetUrlViaName();
        public int? CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public decimal View { get; set; }
        public int? CurrentDuration { get; set; }
    }
}
