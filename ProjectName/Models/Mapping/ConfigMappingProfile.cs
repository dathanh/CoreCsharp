using AutoMapper;
using Framework.Mapping;
using Framework.Utility;
using ProjectName.Models.Banner;
using ProjectName.Models.Config;
using System.IO;
using System.Linq;

namespace ProjectName.Models.Mapping
{
    public class ConfigMappingProfile : Profile
    {
        public ConfigMappingProfile()
        {
            CreateMap<Framework.DomainModel.Entities.Config, DashboardConfigDataViewModel>().AfterMap((s, d) =>
            {
                d.SharedViewModel = s.MapTo<DashboardConfigShareViewModel>();
            });
            CreateMap<Framework.DomainModel.Entities.Config, DashboardConfigShareViewModel>().AfterMap((s, d) =>
            {
                var url = ImageHelper.GetImageUrlCdn();
                d.Background = url + s.Background;
                if (!string.IsNullOrEmpty(s.VideoFile))
                {
                    d.VideoDetails.Add(new VideoItemUpload
                    {
                        FilePath = "/UploadTemp/" + s.VideoFile,
                        FileNameOriginal = s.VideoName
                    });
                }
            });

            CreateMap<DashboardConfigDataViewModel, Framework.DomainModel.Entities.Config>().AfterMap((s, d) =>
            {
                s.SharedViewModel.MapPropertiesToInstance(d);
            });
            CreateMap<DashboardConfigShareViewModel, Framework.DomainModel.Entities.Config>().AfterMap((s, d) =>
            {
                d.Background = Path.GetFileName(s.Background);
                var videoDetail = s.VideoDetails.FirstOrDefault();
                if (videoDetail != null)
                {
                    d.VideoFile = Path.GetFileName(videoDetail.FilePath);
                    d.VideoName = videoDetail.FileNameOriginal;
                }
            });

        }
    }
}