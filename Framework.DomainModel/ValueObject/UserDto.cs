namespace Framework.DomainModel.ValueObject
{
    public class UserDto : DtoBase
    {
        public string UserName { get; set; }

        public string FullName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public int? UserRoleId { get; set; }
        public int? OldUserRoleId { get; set; }
        public string UserRoleName { get; set; }

        public string Password { get; set; }

        public string Avatar { get; set; }

        public byte[] AvatarInBytes { get; set; }
        public bool? IsActive { get; set; }
    }

    public class ChangePasswordDto
    {
        public int? Id { get; set; }
        public string Password { get; set; }
        public string Code { get; set; }

    }
    public class ResetPasswordDto
    {
        public string Email { get; set; }

    }
}