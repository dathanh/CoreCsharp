using Framework.DataAnnotations;

namespace Framework.DomainModel.Entities
{
    public class Config : Entity
    {
        [LocalizeRequired(FieldName = "Name")]
        public string Name { get; set; }
        //public string Value { get; set; }
        public string Description { get; set; }
        public string VideoFile { get; set; }
        public string VideoName { get; set; }
        public string Background { get; set; }
    }
}
