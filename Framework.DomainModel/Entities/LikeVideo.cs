
namespace Framework.DomainModel.Entities
{
    public class LikeVideo : Entity
    {
        public int CustomerId { get; set; }
        public int? VideoId { get; set; }
        public int Type { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Video Video { get; set; }
    }
}