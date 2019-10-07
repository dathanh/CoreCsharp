namespace Framework.DomainModel.Entities
{
    public class VideoLanguage : EntityWithLanguage
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string TimeDuration { get; set; }
        public string Avatar { get; set; }
        public virtual Video Parent { get; set; }
    }
}