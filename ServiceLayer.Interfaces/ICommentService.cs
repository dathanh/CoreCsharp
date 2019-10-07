using Framework.DomainModel.Entities;
using Framework.DomainModel.ValueObject;

namespace ServiceLayer.Interfaces
{
    public interface ICommentService : IMasterFileService<Comment>
    {
        CommentItem PostComment(CommentDto commentInfo);
        CommentListResponse GetCommentByVideoId(int id, int currentCustomerId, int sort, int page);
        CommentListResponse GetCommentByParentId(int parentId, int currentCustomerId, int skip);

    }
}