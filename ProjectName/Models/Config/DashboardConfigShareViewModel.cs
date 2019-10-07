using ProjectName.Models.Base;
using System.Collections.Generic;

namespace ProjectName.Models.Config
{
    public class DashboardConfigShareViewModel : DashboardSharedViewModel
    {
        public string Name { get; set; }
        //public string Value { get; set; }
        public string Description { get; set; }
        public string Background { get; set; }

    }
}