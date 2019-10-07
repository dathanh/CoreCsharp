using Framework.Service.Diagnostics;
using Microsoft.Extensions.Configuration;
using Quartz;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ProjectName.Services.Scheduler
{
    [DisallowConcurrentExecution]
    public class GetViewNumberJobFromYoutube : IJob
    {
        private readonly IConfiguration _configuration;
        public readonly IDiagnosticService _diagnosticService;
        public GetViewNumberJobFromYoutube(IConfiguration configuration, IDiagnosticService diagnosticService)
        {
            _configuration = configuration;
            _diagnosticService = diagnosticService;
        }

        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                using (var webClient = new WebClient())
                {
                    var youtubeLink = _configuration["CurrentHostUrl"] + "Youtube/UpdateViewForVideoData";
                    var data = webClient.DownloadString(youtubeLink);
                }
            }
            catch (Exception ex)
            {
                _diagnosticService.Error(ex);
            }
            return Task.CompletedTask;
        }
    }
}
