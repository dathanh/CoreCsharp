using Framework.DomainModel.ValueObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Interfaces;
using ProjectName.Services.Youtube;
using System.Collections.Generic;
using System.Linq;

namespace ProjectName.Controllers
{
    public class YoutubeController : ControllerBase
    {
        private readonly IVideoService _videoService;
        private readonly IYoutubeService _youtubeService;
        public YoutubeController(IVideoService videoService, IYoutubeService youtubeService)
        {
            _videoService = videoService;
            _youtubeService = youtubeService;
        }

        [AllowAnonymous]
        public bool UpdateViewForVideoData()
        {
            var listVideo = _videoService.ListAll();
            var listDataNeedUpdate = new List<YoutubeViewResult>();
            foreach (var item in listVideo)
            {
                var youtubeData = _youtubeService.GetData(item.UrlLink);
                listDataNeedUpdate.Add(new YoutubeViewResult
                {
                    VideoId = item.Id,
                    View = youtubeData?.Items?.FirstOrDefault()?.Statistics?.ViewCount
                });
            }
            return _videoService.UpdateView(listDataNeedUpdate);
        }
    }
}