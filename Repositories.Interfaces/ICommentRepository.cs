using Framework.DomainModel.Entities;
using Framework.DomainModel.ValueObject;
using Framework.Repositories;

namespace Repositories.Interfaces
{
    public interface ICommentRepository : IEntityFrameworkRepository<Comment>, IQueryableRepository<Comment>
    {
        CommentListResponse GetCommentByVideoId(int id, int currentCustomerId, int sort, int page);
        CommentListResponse GetCommentByParentId(int parentId, int currentCustomerId, int skip);
        CommentItem GetCommentByInfo(CommentDto commentInfo);
        CommentItem GetCommentById(int id, int customerId);
    }
}