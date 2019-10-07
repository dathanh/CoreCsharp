using System.Collections.Generic;

namespace Framework.DomainModel.ValueObject
{
    public class YoutubeData
    {
        public List<YoutubeItem> Items { get; set; }
    }

    public class YoutubeItem
    {
        public YoutubeItemStatistic Statistics { get; set; }
    }

    public class YoutubeItemStatistic
    {
        public int ViewCount { get; set; }
    }

    public class YoutubeViewResult
    {
        public int VideoId { get; set; }
        public int? View { get; set; }
    }
}

