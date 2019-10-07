using Framework.DataAnnotations;
using System.Collections.Generic;

namespace Framework.DomainModel.Entities
{
    public class Series : Entity
    {
        [LocalizeRequired(FieldName = "Name")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Background { get; set; }
        public int? Status { get; set; }
        public int OrderNumber { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<SeriesLanguage> SeriesLanguages { get; set; } = new List<SeriesLanguage>();
        public virtual ICollection<Video> Videos { get; set; } = new List<Video>();
    }
}
