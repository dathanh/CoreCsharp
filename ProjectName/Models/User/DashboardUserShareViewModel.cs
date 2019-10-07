using ProjectName.Models.Base;

namespace ProjectName.Models.User
{
    public class DashboardUserShareViewModel : DashboardSharedViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
        public int UserRoleId { get; set; }
        public bool IsActive { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }


    }

}