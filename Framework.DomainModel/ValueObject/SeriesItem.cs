using System.Collections.Generic;

namespace Framework.DomainModel.ValueObject
{
    public class SeriesItem : DtoBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Background { get; set; }
        public List<VideoItem> ListVideoItems { get; set; }
    }
}
