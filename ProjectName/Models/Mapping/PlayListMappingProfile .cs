using AutoMapper;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.Mapping;
using Framework.Utility;
using ProjectName.Models.PlayList;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ProjectName.Models.Mapping
{
    public class PlayListMappingProfile : Profile
    {
        public PlayListMappingProfile()
        {
            CreateMap<Framework.DomainModel.Entities.PlayList, DashboardPlayListDataViewModel>().AfterMap((s, d) =>
            {
                d.SharedViewModel = s.MapTo<DashboardPlayListShareViewModel>();
            });
            CreateMap<Framework.DomainModel.Entities.PlayList, DashboardPlayListShareViewModel>().AfterMap((s, d) =>
            {
                var url = ImageHelper.GetImageUrlCdn();
                if (s.PlayListLanguages != null && s.PlayListLanguages.Count != 0)
                {
                    var vietnamPlayList =
                        s.PlayListLanguages.FirstOrDefault(o => o.LanguageId == (int)LanguageKey.Vietnamese);
                    if (vietnamPlayList != null)
                    {
                        d.NameInVietnamese = vietnamPlayList.Name;
                        d.DescriptionInVietnamese = vietnamPlayList.Description;
                        d.BackgroundInVietnamese = url + vietnamPlayList.Background;
                    }
                }
                d.SelectedItemIds = s.VideoPlayLists.Select(o => o.VideoId.GetValueOrDefault()).ToList();
                d.Background = url + s.Background;
            });

            CreateMap<DashboardPlayListDataViewModel, Framework.DomainModel.Entities.PlayList>().AfterMap((s, d) =>
            {
                s.SharedViewModel.MapPropertiesToInstance(d);
            });
            CreateMap<DashboardPlayListShareViewModel, Framework.DomainModel.Entities.PlayList>().AfterMap((s, d) =>
            {
                if (d.PlayListLanguages != null && d.PlayListLanguages.Count != 0)
                {
                    var vietnamPlayList =
                        d.PlayListLanguages.FirstOrDefault(o => o.LanguageId == (int)LanguageKey.Vietnamese);
                    if (vietnamPlayList != null)
                    {
                        vietnamPlayList.Name = s.NameInVietnamese;
                        vietnamPlayList.Description = s.DescriptionInVietnamese;
                        vietnamPlayList.Background = Path.GetFileName(s.BackgroundInVietnamese);
                    }
                    else
                    {
                        d.PlayListLanguages.Add(new PlayListLanguage
                        {
                            LanguageId = (int)LanguageKey.Vietnamese,
                            Name = s.NameInVietnamese,
                            Description = s.DescriptionInVietnamese,
                            Background = Path.GetFileName(s.BackgroundInVietnamese),
                        });
                    }
                }
                else
                {
                    d.PlayListLanguages = new List<PlayListLanguage>
                    {
                        new PlayListLanguage
                        {
                            LanguageId = (int) LanguageKey.Vietnamese,
                            Name = s.NameInVietnamese,
                            Description = s.DescriptionInVietnamese,
                            Background = Path.GetFileName(s.BackgroundInVietnamese),
                        }
                    };
                }
                d.Background = Path.GetFileName(s.Background);
            });
        }

    }
}
