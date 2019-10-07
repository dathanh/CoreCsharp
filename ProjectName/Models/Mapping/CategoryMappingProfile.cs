using AutoMapper;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.Mapping;
using Framework.Utility;
using ProjectName.Models.Category;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ProjectName.Models.Mapping
{
    public class CategoryMappingProfile : Profile
    {
        public CategoryMappingProfile()
        {
            CreateMap<Framework.DomainModel.Entities.Category, DashboardCategoryDataViewModel>().AfterMap((s, d) =>
            {
                d.SharedViewModel = s.MapTo<DashboardCategoryShareViewModel>();
            });
            CreateMap<Framework.DomainModel.Entities.Category, DashboardCategoryShareViewModel>().AfterMap((s, d) =>
            {
                var url = ImageHelper.GetImageUrlCdn();
                if (s.CategoryLanguages != null && s.CategoryLanguages.Count != 0)
                {
                    var vietnamCategory =
                        s.CategoryLanguages.FirstOrDefault(o => o.LanguageId == (int)LanguageKey.Vietnamese);
                    if (vietnamCategory != null)
                    {
                        d.NameInVietnamese = vietnamCategory.Name;
                        d.DescriptionInVietnamese = vietnamCategory.Description;
                        d.BackgroundInVietnamese = url + vietnamCategory.Background;
                    }
                }
                //if (!string.IsNullOrWhiteSpace(s.Avatar))
                //{
                //    var url = ImageHelper.GetImageUrlCdn();
                //    d.Avatar = url + s.Avatar;
                //    d.Background = url + s.Background;
                //}
                if (!string.IsNullOrWhiteSpace(s.Background))
                {
                    d.Background = url + s.Background;
                }
                d.ParentName = s.Parent?.Name;
            });

            CreateMap<DashboardCategoryDataViewModel, Framework.DomainModel.Entities.Category>().AfterMap((s, d) =>
            {
                s.SharedViewModel.MapPropertiesToInstance(d);
            });
            CreateMap<DashboardCategoryShareViewModel, Framework.DomainModel.Entities.Category>().AfterMap((s, d) =>
            {
                if (d.CategoryLanguages != null && d.CategoryLanguages.Count != 0)
                {
                    var vietnamCategory =
                        d.CategoryLanguages.FirstOrDefault(o => o.LanguageId == (int)LanguageKey.Vietnamese);
                    if (vietnamCategory != null)
                    {
                        vietnamCategory.Name = s.NameInVietnamese;
                        vietnamCategory.Description = s.DescriptionInVietnamese;
                        vietnamCategory.Background = Path.GetFileName(s.BackgroundInVietnamese);
                    }
                    else
                    {
                        d.CategoryLanguages.Add(new CategoryLanguage
                        {
                            LanguageId = (int)LanguageKey.Vietnamese,
                            Name = s.NameInVietnamese,
                            Background = Path.GetFileName(s.BackgroundInVietnamese),
                            Description = s.DescriptionInVietnamese
                        });
                    }
                }
                else
                {
                    d.CategoryLanguages = new List<CategoryLanguage>
                    {
                        new CategoryLanguage
                        {
                            LanguageId = (int) LanguageKey.Vietnamese,
                            Name = s.NameInVietnamese,
                            Background = Path.GetFileName(s.BackgroundInVietnamese),
                            Description = s.DescriptionInVietnamese
                        }
                    };
                }

                //d.Avatar = Path.GetFileName(s.Avatar);
                d.Background = Path.GetFileName(s.Background);
                if (d.ParentId.GetValueOrDefault() == 0)
                {
                    d.ParentId = null;
                }

                d.Background = Path.GetFileName(s.Background);
                if (d.ParentId.GetValueOrDefault() == 0)
                {
                    d.ParentId = null;
                }
            });
        }
    }
}