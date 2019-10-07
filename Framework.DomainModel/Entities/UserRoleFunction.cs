namespace Framework.DomainModel.Entities
{
    public class UserRoleFunction : Entity
    {
        public int UserRoleId { get; set; }

        public int SecurityOperationId { get; set; }

        public int DocumentTypeId { get; set; }

        public virtual DocumentType DocumentType { get; set; }

        public virtual SecurityOperation SecurityOperation { get; set; }

        public virtual UserRole UserRole { get; set; }
    }
}
