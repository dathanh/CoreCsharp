namespace Framework.DomainModel.ValueObject
{
    public class AddOrRemoveDataToPlayListDto : DtoBase
    {
        public int PlayListId { get; set; }
        public int? VideoId { get; set; }
        public int Type { get; set; }
        public int? BannerId { get; set; }
    }
}

