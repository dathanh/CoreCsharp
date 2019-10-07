using Framework.DomainModel.ValueObject;
using ProjectName.Models.Base;

namespace ProjectName.Models.Role
{
    public class DashboardRoleShareViewModel : DashboardSharedViewModel
    {
        public string Name { get; set; }
        public bool CheckAll { get; set; }
        public string RoleFunctionData { get; set; }
        public string AppRoleName { get; set; }
        public LookupItemVo LookupRole { get; set; }
        public bool IsDefault { get; set; }
    }
}