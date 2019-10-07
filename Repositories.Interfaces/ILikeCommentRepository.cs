using Framework.DomainModel.Entities;
using Framework.Repositories;

namespace Repositories.Interfaces
{
    public interface ILikeCommentRepository : IEntityFrameworkRepository<LikeComment>, IQueryableRepository<LikeComment>
    {
    }
}