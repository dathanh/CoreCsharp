namespace Framework.DomainModel.Entities
{
    public class GridConfig : Entity
    {
        public int DocumentTypeId { get; set; }

        public int UserId { get; set; }

        public string XmlText { get; set; }

        public string GridInternalName { get; set; }

        public virtual DocumentType DocumentType { get; set; }

        public virtual User User { get; set; }
    }
}
