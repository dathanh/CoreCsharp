using Database.Persistance.Tenants;
using Framework.DomainModel.Entities;
using Framework.DomainModel.ValueObject;
using Framework.Utility;
using Repositories.Interfaces;
using System.Linq;

namespace Repositories
{
    public class EntityFrameworkLikeVideoRepository : EntityFrameworkTenantRepositoryBase<LikeVideo>, ILikeVideoRepository
    {
        public EntityFrameworkLikeVideoRepository(ITenantPersistenceService persistenceService)
            : base(persistenceService)
        {

        }
        public int CheckLikeVideoExist(LikeVideo likeInfo)
        {
            var like = DataContext.LikeVideos.FirstOrDefault(l => l.VideoId == likeInfo.VideoId && l.CustomerId == likeInfo.CustomerId);

            return like?.Id ?? 0;
        }
        public LikeVideoResponse GetLikeOfVideo(int videoId, int currentCustomerId)
        {
            var liveOfVideo = new LikeVideoResponse
            {
                CountLike = DataContext.LikeVideos.Count(l => l.VideoId == videoId && l.Type == ConstantValue.Like),
                CountDislike = DataContext.LikeVideos.Count(l => l.VideoId == videoId && l.Type == ConstantValue.Dislike),
                HasLike = DataContext.LikeVideos.Count(l => l.VideoId == videoId && l.Type == ConstantValue.Like && l.CustomerId == currentCustomerId) > 0,
                HasDislike = DataContext.LikeVideos.Count(l => l.VideoId == videoId && l.Type == ConstantValue.Dislike && l.CustomerId == currentCustomerId) > 0
            };
            return liveOfVideo;
        }



    }
}