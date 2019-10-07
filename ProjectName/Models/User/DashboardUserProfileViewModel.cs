using ProjectName.Models.Base;

namespace ProjectName.Models.User
{
    public class DashboardUserProfileViewModel : MasterfileViewModelBase<Framework.DomainModel.Entities.User>
    {
        public Framework.DomainModel.Entities.User User { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserRoleName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string MiddleName { get; set; }
        public string HomePhone { get; set; }
        public string MobilePhone { get; set; }
        public string Avatar { get; set; }

        public bool IsActive { get; set; }

        public string Password { get; set; }
        public int? UserRoleId { get; set; }

        public string OldAvatar { get; set; }
    }
}