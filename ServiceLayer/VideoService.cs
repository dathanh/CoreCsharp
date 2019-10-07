using Database.Persistance.Tenants;
using Framework.BusinessRule;
using Framework.DomainModel.Entities;
using Framework.DomainModel.ValueObject;
using Repositories.Interfaces;
using ServiceLayer.Interfaces;
using System.Collections.Generic;

namespace ServiceLayer
{
    public class VideoService : MasterFileService<Video>, IVideoService
    {
        private readonly IVideoRepository _videoRepository;

        public VideoService(ITenantPersistenceService tenantPersistenceService, IVideoRepository videoRepository,
            IBusinessRuleSet<Video> businessRuleSet = null)
            : base(videoRepository, videoRepository, tenantPersistenceService, businessRuleSet)
        {
            _videoRepository = videoRepository;
        }

        public List<VideoItem> GetListVideoForCustomer(int customerId, int languageId)
        {
            return _videoRepository.GetListVideoForCustomer(customerId, languageId);
        }

        public List<VideoItem> GetListVideoTrending(int languageId)
        {
            return _videoRepository.GetListVideoTrending(languageId);
        }

        public List<VideoItem> GetListVideoPopular(int languageId)
        {
            return _videoRepository.GetListVideoPopular(languageId);
        }

        public List<VideoItem> GetListVideoRecently(int languageId)
        {
            return _videoRepository.GetListVideoRecently(languageId);
        }

        public List<VideoItem> GetVideoContinueWatching(int customerId, int languageId)
        {
            return _videoRepository.GetVideoContinueWatching(customerId, languageId);

        }

        public VideoDetail GetVideoById(int id, int languageId, int customerId)
        {
            return _videoRepository.GetVideoById(id, languageId, customerId);
        }

        public List<VideoItem> GetVideoWatchNext(int videoId, int languageId, int customerId)
        {
            return _videoRepository.GetVideoWatchNext(videoId, languageId, customerId);
        }

        public List<VideoItem> GetVideoMightLove(int videoId, int? page, int languageId)
        {
            return _videoRepository.GetVideoMightLove(videoId, page, languageId);
        }

        public int TotalVideoMightLove(int videoId)
        {
            return _videoRepository.TotalVideoMightLove(videoId);
        }

        public List<VideoSearchItem> Search(string data, int languageId)
        {
            return _videoRepository.Search(data, languageId);
        }

        public bool UpdateView(List<YoutubeViewResult> listDataNeedUpdate)
        {
            return _videoRepository.UpdateView(listDataNeedUpdate);
        }
        public CategoryVideo GetVideoOfCategory(int categoryId, int languageId, int page)
        {
            return _videoRepository.GetVideoOfCategory(categoryId, languageId, page);
        }

        public MetaResponse GetSeoInfo(int videoId, int languageId)
        {
            var videoInfo = _videoRepository.GetVideoById(videoId, languageId);
            if (videoInfo != null)
            {
                return new MetaResponse()
                {
                    Title = videoInfo.Name,
                    Description = videoInfo.Description,
                    Image = videoInfo.Avatar,
                };
            }
            return new MetaResponse();
        }
    }
}