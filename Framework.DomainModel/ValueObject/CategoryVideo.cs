using Framework.Utility;
using System.Collections.Generic;

namespace Framework.DomainModel.ValueObject
{
    public class CategoryVideo
    {
        public string CategoryName { get; set; }
        public string ParentCategory { get; set; }
        public string ParentNameUrl => ParentCategory.GetUrlViaName();
        public int CategoryId { get; set; }
        public int ParentCategoryId { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
        public int TotalRecord { get; set; }
        public bool IsNext
        {
            get
            {
                if (CurrentPage < TotalPage)
                {
                    return true;
                }
                return false;
            }
        }
        public List<VideoItem> ListVideoCategory { get; set; }
    }
}
