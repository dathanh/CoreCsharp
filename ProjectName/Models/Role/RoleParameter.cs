using ProjectName.Models.Base;

namespace ProjectName.Models.Role
{
    public class RoleParameter : MasterfileParameter
    {
        public string RoleFunctionData { get; set; }
        public bool CheckAll { get; set; }
    }
}