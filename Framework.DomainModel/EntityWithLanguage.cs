using Framework.DomainModel.Entities;

namespace Framework.DomainModel
{
    public abstract class EntityWithLanguage : Entity
    {
        public int LanguageId { get; set; }
        public int ParentId { get; set; }
        public virtual Language Language { get; set; }
    }
}