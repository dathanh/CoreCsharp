using System.Collections.Generic;

namespace Framework.DomainModel.ValueObject
{
    public class PlayListItem : CustomerPlaylistForMenu
    {
        public string Description { get; set; }
        public int TotalVideos { get; set; }
        public string Background { get; set; }
        public int RemainCount
        {
            get
            {
                if (TotalVideos > 3)
                {
                    return TotalVideos - 3;
                }
                return 0;
            }
        }
        public List<VideoPlayListItem> ListVideoItems { get; set; } = new List<VideoPlayListItem>();

    }
}
