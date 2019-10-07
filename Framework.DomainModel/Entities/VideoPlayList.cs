namespace Framework.DomainModel.Entities
{
    public class VideoPlayList : Entity
    {
        public int? PlayListId { get; set; }
        public int? VideoId { get; set; }
        public int? BannerId { get; set; }
        public virtual Video Video { get; set; }
        public virtual Banner Banner { get; set; }
        public virtual PlayList PlayList { get; set; }

    }
}
