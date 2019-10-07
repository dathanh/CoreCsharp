namespace Framework.DomainModel.Entities
{
    public class CategoryLanguage : EntityWithLanguage
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Background { get; set; }
        public virtual Category Parent { get; set; }
    }
}