using ProjectName.Models.Base;

namespace ProjectName.Models.User
{
    public class DashboardUserIndexViewModel : DashboardGridViewModelBase<Framework.DomainModel.Entities.User>
    {
        public override string PageTitle => "User";

        public string WebsiteUrl { get; set; }

        public bool CanAddNewRecord { get; set; }
        public bool CanExportExcel { get; set; }
    }

}