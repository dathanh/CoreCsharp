namespace Framework.DomainModel.Entities
{
    public class PlayListLanguage : EntityWithLanguage
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Background { get; set; }
        public virtual PlayList Parent { get; set; }

    }
}
