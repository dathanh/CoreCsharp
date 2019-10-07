namespace Framework.DomainModel.Entities
{
    public class BannerLanguage : EntityWithLanguage
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Background { get; set; }
        public string TimeDuration { get; set; }
        public virtual Banner Parent { get; set; }
    }
}
