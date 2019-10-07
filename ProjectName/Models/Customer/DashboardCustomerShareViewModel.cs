using ProjectName.Models.Banner;
using ProjectName.Models.Base;
using System.Collections.Generic;

namespace ProjectName.Models.Customer
{
    public class DashboardCustomerShareViewModel : DashboardSharedViewModel
    {
        public string Name { get; set; }
        //public string Value { get; set; }
        public string Description { get; set; }
        public string Background { get; set; }
        public List<VideoItemUpload> VideoDetails { get; set; } = new List<VideoItemUpload>();

    }
}