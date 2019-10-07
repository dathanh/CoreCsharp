using Framework.DataAnnotations;
using System.Collections.Generic;

namespace Framework.DomainModel.Entities
{
    public class Category : Entity
    {
        [LocalizeRequired(FieldName = "Name")]
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ParentId { get; set; }
        public bool? IsActive { get; set; }
        public string Background { get; set; }
        public virtual Category Parent { get; set; }
        public virtual ICollection<Category> Children { get; set; } = new List<Category>();
        public virtual ICollection<CategoryLanguage> CategoryLanguages { get; set; } = new List<CategoryLanguage>();
        public virtual ICollection<Video> Videos { get; set; } = new List<Video>();
    }
}