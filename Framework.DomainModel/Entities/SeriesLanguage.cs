namespace Framework.DomainModel.Entities
{
    public class SeriesLanguage : EntityWithLanguage
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Background { get; set; }
        public virtual Series Parent { get; set; }

    }
}
