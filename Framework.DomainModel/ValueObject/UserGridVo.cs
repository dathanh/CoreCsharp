using Framework.Utility;

namespace Framework.DomainModel.ValueObject
{
    public class UserGridVo : ReadOnlyGridVo
    {
        public string UserName { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string FullName { get; set; }
        public bool IsActive { get; set; }

        public string PhoneInFormat => Phone.ApplyFormatPhone();
    }
}