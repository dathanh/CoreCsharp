using Framework.DataAnnotations;
using System.Collections.Generic;

namespace Framework.DomainModel.Entities
{
    public class Comment : Entity
    {
        [LocalizeRequired(FieldName = "Message")]
        public string Message { get; set; } 
        public int CustomerId { get; set; }
        public int VideoId { get; set; }
        public int? ParentId { get; set; }
        public bool? IsActive { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Video Video { get; set; }
        public virtual Comment Parent { get; set; }
        public virtual ICollection<Comment> Children { get; set; } = new List<Comment>();
        public virtual ICollection<LikeComment> LikeComments { get; set; } = new List<LikeComment>();
    }
}