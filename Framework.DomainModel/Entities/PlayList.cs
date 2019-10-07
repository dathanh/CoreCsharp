using Framework.DataAnnotations;
using System.Collections.Generic;

namespace Framework.DomainModel.Entities
{
    public class PlayList : Entity
    {
        [LocalizeRequired(FieldName = "Name")]
        public string Name { get; set; }
        public int? OwnerCustomerId { get; set; }
        public string Description { get; set; }
        public int OrderNumber { get; set; }
        public bool IsActive { get; set; }
        public virtual Customer Customer { get; set; }
        public string Background { get; set; }

        public virtual ICollection<PlayListLanguage> PlayListLanguages { get; set; } = new List<PlayListLanguage>();
        public virtual ICollection<VideoPlayList> VideoPlayLists { get; set; } = new List<VideoPlayList>();
    }
}
