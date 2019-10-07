namespace Framework.DomainModel.ValueObject
{
    public class UserRoleFunctionGridVo : DtoBase
    {
        public string Name { get; set; }
        public bool IsView { get; set; }
        public bool IsInsert { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsDelete { get; set; }
        public bool IsProcess { get; set; }
        public bool IsShowMenu { get; set; }
    }
}