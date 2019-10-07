using Framework.DomainModel.ValueObject;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net;

namespace ProjectName.Services.Youtube
{
    public class YoutubeService : IYoutubeService
    {
        private readonly IConfiguration _configuration;
        public YoutubeService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public YoutubeData GetData(string urlLink)
        {
            var youtubeLinkFormat = _configuration["YoutubeLink"];
            if (string.IsNullOrWhiteSpace(youtubeLinkFormat))
            {
                return null;
            }
            var youtubeLink = string.Format(youtubeLinkFormat, urlLink);
            var youtubeContent = "";
            try
            {
                using (var webClient = new WebClient())
                {
                    youtubeContent = webClient.DownloadString(youtubeLink);
                }
            }
            catch
            {
                youtubeContent = "";
            }
            if (string.IsNullOrWhiteSpace(youtubeContent))
            {
                return null;
            }
            var youtubeData = JsonConvert.DeserializeObject<YoutubeData>(youtubeContent);

            return youtubeData;
        }
    }

    public interface IYoutubeService
    {
        YoutubeData GetData(string urlLink);
    }
}
