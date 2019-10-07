using ProjectName.Models.Base;
using System.Collections.Generic;

namespace ProjectName.Models.PlayList
{
    public class DashboardPlayListShareViewModel : DashboardSharedViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string NameInVietnamese { get; set; }
        public string DescriptionInVietnamese { get; set; }
        public string BackgroundInVietnamese { get; set; }
        public List<int> SelectedItemIds { get; set; } = new List<int>();
        public int OrderNumber { get; set; }
        public bool IsActive { get; set; }
        public string Background { get; set; }
    }
}
