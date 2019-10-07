using AutoMapper;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.Mapping;
using Framework.Utility;
using ProjectName.Models.Series;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ProjectName.Models.Mapping
{
    public class SeriesMappingProfile : Profile
    {
        public SeriesMappingProfile()
        {
            CreateMap<Framework.DomainModel.Entities.Series, DashboardSeriesDataViewModel>().AfterMap((s, d) =>
            {
                d.SharedViewModel = s.MapTo<DashboardSeriesShareViewModel>();
            });
            CreateMap<Framework.DomainModel.Entities.Series, DashboardSeriesShareViewModel>().AfterMap((s, d) =>
            {
                var url = ImageHelper.GetImageUrlCdn();
                if (s.SeriesLanguages != null && s.SeriesLanguages.Count != 0)
                {
                    var vietnamSeries =
                        s.SeriesLanguages.FirstOrDefault(o => o.LanguageId == (int)LanguageKey.Vietnamese);
                    if (vietnamSeries != null)
                    {
                        d.NameInVietnamese = vietnamSeries.Name;
                        d.DescriptionInVietnamese = vietnamSeries.Description;
                        d.BackgroundInVietnamese = url + vietnamSeries.Background;
                    }
                }
                d.Background = url + s.Background;
                d.SelectedItemIds = s.Videos.Select(o => o.Id).ToList();
            });

            CreateMap<DashboardSeriesDataViewModel, Framework.DomainModel.Entities.Series>().AfterMap((s, d) =>
            {
                s.SharedViewModel.MapPropertiesToInstance(d);
            });
            CreateMap<DashboardSeriesShareViewModel, Framework.DomainModel.Entities.Series>().AfterMap((s, d) =>
            {
                if (d.SeriesLanguages != null && d.SeriesLanguages.Count != 0)
                {
                    var vietnamSeries =
                        d.SeriesLanguages.FirstOrDefault(o => o.LanguageId == (int)LanguageKey.Vietnamese);
                    if (vietnamSeries != null)
                    {
                        vietnamSeries.Name = s.NameInVietnamese;
                        vietnamSeries.Description = s.DescriptionInVietnamese;
                        vietnamSeries.Background = Path.GetFileName(s.BackgroundInVietnamese);
                    }
                    else
                    {
                        d.SeriesLanguages.Add(new SeriesLanguage
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
                    d.SeriesLanguages = new List<SeriesLanguage>
                    {
                        new SeriesLanguage
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
