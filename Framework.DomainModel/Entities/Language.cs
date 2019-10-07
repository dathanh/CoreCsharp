using System.Collections.Generic;

namespace Framework.DomainModel.Entities
{
    public class Language : Entity
    {
        public string Name { get; set; }
        public bool? IsDefault { get; set; }

        public virtual ICollection<CategoryLanguage> CategoryLanguages { get; set; } = new List<CategoryLanguage>();
        public virtual ICollection<VideoLanguage> VideoLanguages { get; set; } = new List<VideoLanguage>();
        public virtual ICollection<BannerLanguage> BannerLanguages { get; set; } = new List<BannerLanguage>();
        public virtual ICollection<SeriesLanguage> SeriesLanguages { set; get; } = new List<SeriesLanguage>();
        public virtual ICollection<PlayListLanguage> PlayListLanguages { set; get; } = new List<PlayListLanguage>();
        public virtual ICollection<Customer> Customers { set; get; } = new List<Customer>();
    }
}