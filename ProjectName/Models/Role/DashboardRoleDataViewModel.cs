using ProjectName.Models.Base;

namespace ProjectName.Models.Role
{
    public class DashboardRoleDataViewModel : MasterfileViewModelBase<Framework.DomainModel.Entities.UserRole>
    {
        public override void MapFromClientParameters(MasterfileParameter parameters)
        {
            SharedViewModel = MapFromClientParameters<DashboardRoleShareViewModel>(parameters);
        }

        public override string PageTitle => SharedViewModel.CreateMode ? "Create Role" : "Update Role";
    }
}