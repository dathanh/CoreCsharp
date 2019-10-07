
using AutoMapper;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.Mapping;
using Framework.Utility;
using ProjectName.Models.Video;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ProjectName.Models.Mapping
{
    public class VideoMappingProfile : Profile
    {
        public VideoMappingProfile()
        {
            CreateMap<Framework.DomainModel.Entities.Video, DashboardVideoDataViewModel>().AfterMap((s, d) =>
            {
                d.SharedViewModel = s.MapTo<DashboardVideoShareViewModel>();
            });
            CreateMap<Framework.DomainModel.Entities.Video, DashboardVideoShareViewModel>().AfterMap((s, d) =>
            {
                var url = ImageHelper.GetImageUrlCdn();
                if (s.VideoLanguages != null && s.VideoLanguages.Count != 0)
                {
                    var vietnamVideo =
                        s.VideoLanguages.FirstOrDefault(o => o.LanguageId == (int)LanguageKey.Vietnamese);
                    if (vietnamVideo != null)
                    {
                        d.NameInVietnamese = vietnamVideo.Name;
                        d.DescriptionInVietnamese = vietnamVideo.Description;
                        d.TimeDurationInVietnamese = vietnamVideo.TimeDuration;
                        d.AvatarInVietnamese = url + vietnamVideo.Avatar;
                    }
                }
                if (!string.IsNullOrWhiteSpace(s.Avatar))
                {
                    d.Avatar = url + s.Avatar;
                }
                d.SeriesName = s.Series?.Name;
                d.CategoryName = s.Category?.Name;
                d.ParentCategoryName = s.Category?.Parent?.Name;
                d.ParentCategoryId = s.Category?.ParentId;
            });

            CreateMap<DashboardVideoDataViewModel, Framework.DomainModel.Entities.Video>().AfterMap((s, d) =>
            {
                s.SharedViewModel.MapPropertiesToInstance(d);
            });
            CreateMap<DashboardVideoShareViewModel, Framework.DomainModel.Entities.Video>().AfterMap((s, d) =>
            {
                if (d.VideoLanguages != null && d.VideoLanguages.Count != 0)
                {
                    var vietnamVideo =
                        d.VideoLanguages.FirstOrDefault(o => o.LanguageId == (int)LanguageKey.Vietnamese);
                    if (vietnamVideo != null)
                    {
                        vietnamVideo.Name = s.NameInVietnamese;
                        vietnamVideo.Description = s.DescriptionInVietnamese;
                        vietnamVideo.TimeDuration = s.TimeDurationInVietnamese;
                        vietnamVideo.Avatar = Path.GetFileName(s.AvatarInVietnamese);
                    }
                    else
                    {
                        d.VideoLanguages.Add(new VideoLanguage
                        {
                            LanguageId = (int)LanguageKey.Vietnamese,
                            Name = s.NameInVietnamese,
                            Description = s.DescriptionInVietnamese,
                            Avatar = Path.GetFileName(s.AvatarInVietnamese),
                            TimeDuration = s.TimeDurationInVietnamese
                        });
                    }
                }
                else
                {
                    d.VideoLanguages = new List<VideoLanguage>
                    {
                        new VideoLanguage
                        {
                            LanguageId = (int) LanguageKey.Vietnamese,
                            Name = s.NameInVietnamese,
                            Description = s.DescriptionInVietnamese,
                            Avatar = Path.GetFileName(s.AvatarInVietnamese),
                            TimeDuration = s.TimeDurationInVietnamese
                        }
                    };
                }

                d.Avatar = Path.GetFileName(s.Avatar);
                if (d.SeriesId.GetValueOrDefault() == 0)
                {
                    d.SeriesId = null;
                }
                if (d.CategoryId.GetValueOrDefault() == 0)
                {
                    d.CategoryId = null;
                }

            });
        }
    }
}