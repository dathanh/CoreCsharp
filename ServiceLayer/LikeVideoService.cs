using Database.Persistance.Tenants;
using Framework.BusinessRule;
using Framework.DomainModel.Entities;
using Framework.DomainModel.ValueObject;
using Framework.Utility;
using Repositories.Interfaces;
using ServiceLayer.Interfaces;

namespace ServiceLayer
{
    public class LikeVideoService : MasterFileService<LikeVideo>, ILikeVideoService
    {
        private readonly ILikeVideoRepository _likeVideoRepository;

        public LikeVideoService(ITenantPersistenceService tenantPersistenceService, ILikeVideoRepository likeVideoRepository,
            IBusinessRuleSet<LikeVideo> businessRuleSet = null)
            : base(likeVideoRepository, likeVideoRepository, tenantPersistenceService, businessRuleSet)
        {
            _likeVideoRepository = likeVideoRepository;
        }

        public LikeVideoResponse ProcessLikeVideo(LikeVideo likeVideoInfo)
        {
            var likeExist = _likeVideoRepository.FirstOrDefault(o => o.VideoId == likeVideoInfo.VideoId && o.CustomerId == likeVideoInfo.CustomerId);
            if (likeExist == null)
            {
                _likeVideoRepository.Add(new LikeVideo
                {
                    VideoId = likeVideoInfo.VideoId,
                    CustomerId = likeVideoInfo.CustomerId,
                    Type = likeVideoInfo.Type,
                });
            }
            else
            {
                if (likeExist.Type == likeVideoInfo.Type)
                {
                    _likeVideoRepository.Remove(likeExist);
                }
                else
                {
                    likeExist.Type = likeVideoInfo.Type;
                }
            }
            _likeVideoRepository.Commit();
            return _likeVideoRepository.GetLikeOfVideo(likeVideoInfo.VideoId.GetValueOrDefault(), likeVideoInfo.CustomerId);
        }
        public LikeVideoResponse GetLikeOfVideo(int videoId, int currentCustomerId)
        {
            return _likeVideoRepository.GetLikeOfVideo(videoId, currentCustomerId);
        }

    }
}