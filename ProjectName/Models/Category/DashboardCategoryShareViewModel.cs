using ProjectName.Models.Base;

namespace ProjectName.Models.Category
{
    public class DashboardCategoryShareViewModel : DashboardSharedViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string NameInVietnamese { get; set; }
        public string DescriptionInVietnamese { get; set; }
        public string BackgroundInVietnamese { get; set; }
        public int? ParentId { get; set; }
        public bool? IsActive { get; set; }
        //public string Avatar { get; set; }
        public string Background { get; set; }

        public string ParentName { get; set; }
    }
}