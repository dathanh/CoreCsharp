namespace Framework.DomainModel.ValueObject
{
    public class UserRoleGridVo : ReadOnlyGridVo
    {
        public string Name { get; set; }
        public string AppRoleName { get; set; }
    }
}