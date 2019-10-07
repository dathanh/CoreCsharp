using ProjectName.Models.Base;

namespace ProjectName.Models.Role
{
    public class DashboardRoleIndexViewModel : DashboardGridViewModelBase<Framework.DomainModel.Entities.UserRole>
    {
        public override string PageTitle => "Roles";

        public bool CheckIsAppRole(int idRole)
        {
            return true;
        }

        public bool CanEditRecord { get; set; }
        public bool CanDeleteRecord { get; set; }
    }
}