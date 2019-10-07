
using System.Collections.Generic;

namespace Framework.DomainModel.ValueObject
{
    public class PlaylistDetail : CustomerPlaylistForMenu
    {
        public string Description { get; set; }
        public int TotalVideos { get; set; }
        public string Background { get; set; }
        public List<VideoPlayListItem> ListVideoItems { get; set; } = new List<VideoPlayListItem>();
        public List<VideoPlayListItemDetail> ListVideoItemDetails { get; set; } = new List<VideoPlayListItemDetail>();
    }
}
