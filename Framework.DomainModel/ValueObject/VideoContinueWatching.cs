
namespace Framework.DomainModel.ValueObject
{
    public class VideoContinueWatching : DtoBase
    {
        public int? VideoId { get; set; }
        public int? CustomerId { get; set; }
        public int? CurrentDuration { get; set; }
        public int Status { get; set; }


    }
}
