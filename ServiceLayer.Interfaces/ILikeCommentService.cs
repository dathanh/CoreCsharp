using Framework.DomainModel.Entities;
using Framework.DomainModel.ValueObject;

namespace ServiceLayer.Interfaces
{
    public interface ILikeCommentService : IMasterFileService<LikeComment>
    {
        CommentItem ProcessLikeComment(LikeComment commentInfo);
    }
}