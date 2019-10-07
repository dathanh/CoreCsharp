using AutoMapper;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.Mapping;
using Framework.Utility;
using ProjectName.Models.Banner;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ProjectName.Models.Mapping
{
    public class BannerMappingProfile : Profile
    {
        public BannerMappingProfile()
        {
            CreateMap<Framework.DomainModel.Entities.Banner, DashboardBannerDataViewModel>().AfterMap((s, d) =>
            {
                d.SharedViewModel = s.MapTo<DashboardBannerShareViewModel>();
            });
            CreateMap<Framework.DomainModel.Entities.Banner, DashboardBannerShareViewModel>().AfterMap((s, d) =>
            {
                var url = ImageHelper.GetImageUrlCdn();
                if (s.BannerLanguages != null && s.BannerLanguages.Count != 0)
                {
                    var vietnamBanner =
                        s.BannerLanguages.FirstOrDefault(o => o.LanguageId == (int)LanguageKey.Vietnamese);
                    if (vietnamBanner != null)
                    {
                        d.NameInVietnamese = vietnamBanner.Name;
                        d.DescriptionInVietnamese = vietnamBanner.Description;
                        d.BackgroundInVietnamese = url + vietnamBanner.Background;
                        d.TimeDurationInVietnamese = vietnamBanner.TimeDuration;
                    }
                }
                d.Background = url + s.Background;
                if (s.Type == (int)BannerTypeEnum.BannerTop || s.Type == (int)BannerTypeEnum.AdvertisementBanner)
                {
                    d.VideoId = s.VideoId;
                    d.UrlLink = "";
                    d.VideoName = s.Video?.Name;
                    if (!string.IsNullOrEmpty(s.VideoFile))
                    {
                        d.VideoDetails.Add(new VideoItemUpload
                        {
                            FileNameOriginal = s.VideoOriginName,
                            FilePath = "/UploadTemp/" + s.VideoFile,
                            VideoLink = url + s.VideoFile
                        });
                    }

                }
                else
                {
                    d.VideoId = null;
                    d.UrlLink = s.UrlLink;
                    d.UrlLink = s.UrlLink;
                }
            });

            CreateMap<DashboardBannerDataViewModel, Framework.DomainModel.Entities.Banner>().AfterMap((s, d) =>
            {
                s.SharedViewModel.MapPropertiesToInstance(d);
            });
            CreateMap<DashboardBannerShareViewModel, Framework.DomainModel.Entities.Banner>().AfterMap((s, d) =>
            {
                if (d.BannerLanguages != null && d.BannerLanguages.Count != 0)
                {
                    var vietnamBanner =
                        d.BannerLanguages.FirstOrDefault(o => o.LanguageId == (int)LanguageKey.Vietnamese);
                    if (vietnamBanner != null)
                    {
                        vietnamBanner.Name = s.NameInVietnamese;
                        vietnamBanner.Description = s.DescriptionInVietnamese;
                        vietnamBanner.Background = Path.GetFileName(s.BackgroundInVietnamese);
                        vietnamBanner.TimeDuration = s.TimeDurationInVietnamese;
                    }
                    else
                    {
                        d.BannerLanguages.Add(new BannerLanguage
                        {
                            LanguageId = (int)LanguageKey.Vietnamese,
                            Name = s.NameInVietnamese,
                            Description = s.DescriptionInVietnamese,
                            Background = Path.GetFileName(s.BackgroundInVietnamese),
                            TimeDuration = s.TimeDurationInVietnamese,
                        });
                    }
                }
                else
                {
                    d.BannerLanguages = new List<BannerLanguage>
                    {
                        new BannerLanguage
                        {
                            LanguageId = (int) LanguageKey.Vietnamese,
                            Name = s.NameInVietnamese,
                            Description = s.DescriptionInVietnamese,
                            Background = Path.GetFileName(s.BackgroundInVietnamese),
                            TimeDuration = s.TimeDurationInVietnamese,
                        }
                    };
                }

                d.Background = Path.GetFileName(s.Background);
                if (s.Type == (int)BannerTypeEnum.BannerTop || s.Type == (int)BannerTypeEnum.AdvertisementBanner)
                {
                    d.UrlLink = "";
                    d.VideoId = s.VideoId;
                    var videoDetail = s.VideoDetails.FirstOrDefault();
                    if (videoDetail != null)
                    {
                        d.VideoFile = Path.GetFileName(videoDetail.FilePath);
                        d.VideoOriginName = videoDetail.FileNameOriginal;
                    }
                }
                else
                {
                    d.UrlLink = s.UrlLink;
                    s.VideoId = null;
                }
            });
        }
    }
}