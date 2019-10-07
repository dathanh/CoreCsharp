using Framework.DomainModel.Entities;
using Framework.DomainModel.ValueObject;
using Framework.Repositories;
using System.Collections.Generic;

namespace Repositories.Interfaces
{
    public interface IVideoRepository : IEntityFrameworkRepository<Video>, IQueryableRepository<Video>
    {
        bool CheckNameLanguageIsExists(int videoLanguageId, string name, int languageId);
        List<VideoItem> GetListVideoForCustomer(int customerId, int languageId);
        List<VideoItem> GetVideoContinueWatching(int customerId, int languageId);
        List<VideoItem> GetListVideoTrending(int languageId);
        List<VideoItem> GetListVideoPopular(int languageId);
        List<VideoItem> GetListVideoRecently(int languageId);
        VideoDetail GetVideoById(int id, int languageId, int customerId);
        List<VideoItem> GetVideoWatchNext(int videoId, int languageId, int customerId);
        List<VideoItem> GetVideoMightLove(int videoId, int? page, int languageId);
        int TotalVideoMightLove(int videoId);
        List<VideoSearchItem> Search(string data, int languageId);
        bool UpdateView(List<YoutubeViewResult> listDataNeedUpdate);
        CategoryVideo GetVideoOfCategory(int categoryId, int languageId, int page);
        VideoItem GetVideoById(int videoId, int languageId);

    }
}