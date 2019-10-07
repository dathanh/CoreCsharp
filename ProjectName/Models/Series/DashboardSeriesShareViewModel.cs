using ProjectName.Models.Base;
using System.Collections.Generic;

namespace ProjectName.Models.Series
{
    public class DashboardSeriesShareViewModel : DashboardSharedViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string NameInVietnamese { get; set; }
        public string DescriptionInVietnamese { get; set; }
        public string BackgroundInVietnamese { get; set; }
        public string Background { get; set; }
        public int? Status { get; set; }
        public int OrderNumber { get; set; }
        public bool IsActive { get; set; }
        public List<int> SelectedItemIds { get; set; }
    }
}
