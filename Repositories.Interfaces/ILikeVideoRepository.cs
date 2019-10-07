using Framework.DomainModel.Entities;
using Framework.DomainModel.ValueObject;
using Framework.Repositories;

namespace Repositories.Interfaces
{
    public interface ILikeVideoRepository : IEntityFrameworkRepository<LikeVideo>, IQueryableRepository<LikeVideo>
    {
        int CheckLikeVideoExist(LikeVideo likeInfo);
        LikeVideoResponse GetLikeOfVideo(int videoId, int currentCustomerId);
    }
}