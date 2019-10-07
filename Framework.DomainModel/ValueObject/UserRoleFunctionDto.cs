namespace Framework.DomainModel.ValueObject
{
    public class UserRoleFunctionDto : DtoBase
    {
        public int UserRoleId { get; set; }

        public int SecurityOperationId { get; set; }

        public int DocumentTypeId { get; set; }
    }
}