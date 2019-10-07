namespace Framework.DomainModel.ValueObject
{
    public class YourPlayListDto : CustomerPlaylistForMenu
    {
        public string Background => string.IsNullOrWhiteSpace(BackgroundVideo) ? BackgroundUrl : BackgroundVideo;

        public int TotalAll { get; set; }
        public string BackgroundVideo { get; set; }
        public string BackgroundUrl { get; set; }
        public string Description { get; set; }

    }
}

