
namespace Framework.DomainModel.Entities
{
    public class LikeComment : Entity
    {
        public int CustomerId { get; set; }
        public int CommentId { get; set; }
        public int Type { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Comment Comment { get; set; }

    }
}