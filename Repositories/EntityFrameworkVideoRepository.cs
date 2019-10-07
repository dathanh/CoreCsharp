using Database.Persistance.Tenants;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.ValueObject;
using Framework.Utility;
using Newtonsoft.Json;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Repositories
{
    public class EntityFrameworkVideoRepository : EntityFrameworkTenantRepositoryBase<Video>, IVideoRepository
    {
        public EntityFrameworkVideoRepository(ITenantPersistenceService persistenceService)
            : base(persistenceService)
        {
            SearchColumns.Add("Name");
            SearchColumns.Add("Category");
            DisplayColumnForCombobox.Add("Name");
        }

        public override IQueryable<ReadOnlyGridVo> BuildQueryToGetDataForGrid(IQueryInfo queryInfo)
        {
            var query = queryInfo as VideoQuery ?? new VideoQuery();
            var allData = GetAll();
            if (query.CategoryId.GetValueOrDefault() != 0)
            {
                allData = allData.Where(o => o.CategoryId == query.CategoryId);
            }
            else if (query.ParentCategoryId.GetValueOrDefault() != 0)
            {
                allData = allData.Where(o => o.Category != null && o.Category.ParentId == query.ParentCategoryId);
            }
            if (query.SeriesId.GetValueOrDefault() != 0)
            {
                allData = allData.Where(o => o.SeriesId == query.SeriesId);
            }
            return (from s in allData
                    select new VideoGridVo
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Description = s.Description,
                        IsPopular = s.IsPopular,
                        Avatar = s.Avatar,
                        IsTrending = s.IsTrending,
                        Category = s.Category != null ? s.Category.Name : "",
                        IsActive = s.IsActive,
                        TimeDuration = s.TimeDuration,
                        Series = s.Series == null ? "" : s.Series.Name,
                        ParentCategory = s.Category != null ? (s.Category.Parent != null ? s.Category.Parent.Name : "") : ""
                    }).OrderBy(queryInfo.SortString);
        }

        public bool CheckNameLanguageIsExists(int videoLanguageId, string name, int languageId)
        {
            return (from o in DataContext.VideoLanguages
                    where o.LanguageId == languageId && o.Id != videoLanguageId && o.Name != null &&
                          o.Name.ToLower() == name.ToLower()
                    select o).Any();
        }

        public List<VideoItem> GetListVideoForCustomer(int customerId, int languageId)
        {
            var userData = DataContext.Customers.FirstOrDefault(o => o.Id == customerId);
            if (userData == null)
            {
                return new List<VideoItem>();
            }
            var configStr = userData.CategoryConfig;
            var customerConfigData = new List<CustomerCategoryConfigDto>();
            if (!string.IsNullOrWhiteSpace(configStr))
            {
                customerConfigData = JsonConvert.DeserializeObject<List<CustomerCategoryConfigDto>>(configStr) ??
                                     new List<CustomerCategoryConfigDto>();
            }
            var listSubCategoryId = new List<int?>();
            foreach (var item in customerConfigData)
            {
                listSubCategoryId.AddRange(item.SubCategoryIds);
            }

            var urlCdn = ImageHelper.GetImageUrlCdn();
            IQueryable<VideoItem> query;
            var videoConditionData = DataContext.Videos.Where(
                             // Video should be active
                             v => v.IsActive
                             // Video not watching by customer
                             && (v.CustomerVideoWatcheds.Any(cv => cv.Status == (int)CustomerVideoStatus.Watching && cv.CustomerId == customerId) == false)
                             // Video in list category of customer setup
                             && ((listSubCategoryId.Any() && listSubCategoryId.Contains(v.CategoryId)) || !listSubCategoryId.Any())
                             );
            if (languageId == (int)LanguageKey.English)
            {
                query = (from v in videoConditionData
                         select new VideoItem
                         {
                             Id = v.Id,
                             Name = v.Name,
                             Avatar = urlCdn + v.Avatar,
                             UrlLink = v.UrlLink,
                             TimeDuration = v.TimeDuration,
                             Description = v.Description
                         });
            }
            else
            {
                query = (from v in videoConditionData
                         join vl in DataContext.VideoLanguages.Where(vl => vl.LanguageId == languageId) on v.Id equals vl
                             .ParentId
                         select new VideoItem
                         {
                             Id = v.Id,
                             Name = vl.Name,
                             Avatar = string.IsNullOrWhiteSpace(vl.Avatar) ? urlCdn + v.Avatar : urlCdn + vl.Avatar,
                             UrlLink = v.UrlLink,
                             TimeDuration = vl.TimeDuration,
                             Description = vl.Description
                         });
            }
            var result = query.AsNoTracking().ToList();
            result = result.OrderBy(o => Guid.NewGuid()).ToList().Take(ConstantValue.LimitVideoForUser).ToList();
            return result;
        }


        public List<VideoItem> GetVideoContinueWatching(int customerId, int languageId)
        {
            var urlCdn = ImageHelper.GetImageUrlCdn();
            var videoConditionData = from v in DataContext.Videos.Where(v => v.IsActive)
                                     join cv in DataContext.CustomerVideoWatcheds.Where(cv => cv.Status == (int)CustomerVideoStatus.Watching && cv.CustomerId == customerId)
                                     on v.Id equals cv.VideoId
                                     select v;
            IQueryable<VideoItem> query;
            if (languageId == (int)LanguageKey.English)
            {
                query = from v in videoConditionData
                        select new VideoItem
                        {
                            Id = v.Id,
                            Name = v.Name,
                            Avatar = urlCdn + v.Avatar,
                            UrlLink = v.UrlLink,
                            TimeDuration = v.TimeDuration,
                            Description = v.Description
                        };
            }
            else
            {
                query = (from v in videoConditionData
                         join vl in DataContext.VideoLanguages.Where(vl => vl.LanguageId == languageId) on v.Id equals vl.ParentId
                         select new VideoItem
                         {
                             Id = v.Id,
                             Name = vl.Name,
                             Avatar = string.IsNullOrWhiteSpace(vl.Avatar) ? urlCdn + v.Avatar : urlCdn + vl.Avatar,
                             UrlLink = v.UrlLink,
                             TimeDuration = vl.TimeDuration,
                             Description = vl.Description
                         });
            }
            return query.OrderByDescending(v => v.Id).Take(ConstantValue.LimitVideoContinueWatching).AsNoTracking().ToList();
        }

        public List<VideoItem> GetListVideoTrending(int languageId)
        {
            var urlCdn = ImageHelper.GetImageUrlCdn();
            var videoCondition = from v in DataContext.Videos.Where(v => v.IsActive && v.IsTrending == true) select v;
            IQueryable<VideoItem> query;
            if (languageId == (int)LanguageKey.English)
            {
                query = from v in videoCondition
                        select new VideoItem
                        {
                            Id = v.Id,
                            Name = v.Name,
                            Avatar = urlCdn + v.Avatar,
                            UrlLink = v.UrlLink,
                            TimeDuration = v.TimeDuration,
                            Description = v.Description
                        };
            }
            else
            {
                query = from v in videoCondition
                        join vl in DataContext.VideoLanguages.Where(vl => vl.LanguageId == languageId) on v.Id equals vl.ParentId
                        select new VideoItem
                        {
                            Id = v.Id,
                            Name = vl.Name,
                            Avatar = string.IsNullOrWhiteSpace(vl.Avatar) ? urlCdn + v.Avatar : urlCdn + vl.Avatar,
                            UrlLink = v.UrlLink,
                            TimeDuration = vl.TimeDuration,
                            Description = vl.Description
                        };
            }
            return query.OrderByDescending(v => v.Id).Take(ConstantValue.LimitVideoTrending).AsNoTracking().ToList();
        }

        public List<VideoItem> GetListVideoPopular(int languageId)
        {
            var urlCdn = ImageHelper.GetImageUrlCdn();
            var videoCondition = from v in DataContext.Videos.Where(v => v.IsActive && v.IsPopular == true) select v;
            IQueryable<VideoItem> query;
            if (languageId == (int)LanguageKey.English)
            {
                query = from v in videoCondition
                        select new VideoItem
                        {
                            Id = v.Id,
                            Name = v.Name,
                            Avatar = urlCdn + v.Avatar,
                            UrlLink = v.UrlLink,
                            TimeDuration = v.TimeDuration,
                            Description = v.Description
                        };
            }
            else
            {
                query = from v in videoCondition
                        join vl in DataContext.VideoLanguages.Where(vl => vl.LanguageId == languageId) on v.Id equals vl.ParentId
                        select new VideoItem
                        {
                            Id = v.Id,
                            Name = vl.Name,
                            Avatar = string.IsNullOrWhiteSpace(vl.Avatar) ? urlCdn + v.Avatar : urlCdn + vl.Avatar,
                            UrlLink = v.UrlLink,
                            TimeDuration = vl.TimeDuration,
                            Description = vl.Description
                        };
            }

            return query.OrderByDescending(v => v.Id).Take(ConstantValue.LimitVideoPopular).AsNoTracking().ToList();

        }

        public List<VideoItem> GetListVideoRecently(int languageId)
        {
            var urlCdn = ImageHelper.GetImageUrlCdn();
            var videoCondition = from v in DataContext.Videos.Where(v => v.IsActive) select v;
            IQueryable<VideoItem> query;
            if (languageId == (int)LanguageKey.English)
            {
                query = from v in videoCondition
                        select new VideoItem
                        {
                            Id = v.Id,
                            Name = v.Name,
                            Avatar = urlCdn + v.Avatar,
                            UrlLink = v.UrlLink,
                            TimeDuration = v.TimeDuration,
                            Description = v.Description
                        };
            }
            else
            {
                query = from v in videoCondition
                        join vl in DataContext.VideoLanguages.Where(vl => vl.LanguageId == languageId) on v.Id equals vl.ParentId
                        select new VideoItem
                        {
                            Id = v.Id,
                            Name = vl.Name,
                            Avatar = string.IsNullOrWhiteSpace(vl.Avatar) ? urlCdn + v.Avatar : urlCdn + vl.Avatar,
                            UrlLink = v.UrlLink,
                            TimeDuration = vl.TimeDuration,
                            Description = vl.Description
                        };
            }

            return query.OrderByDescending(v => v.Id).Take(ConstantValue.LimitVideoRecently).AsNoTracking().ToList();
        }

        public VideoDetail GetVideoById(int id, int languageId, int customerId)
        {
            var urlCdn = ImageHelper.GetImageUrlCdn();
            var videoCondition = from v in DataContext.Videos.Where(v => v.IsActive && v.Id == id) select v;
            IQueryable<VideoDetail> query;

            if (languageId == (int)LanguageKey.English)
            {
                query = from v in videoCondition
                        join cw in DataContext.CustomerVideoWatcheds.Where(cw => cw.CustomerId == customerId) on v.Id equals cw.VideoId into cws
                        from cw in cws.DefaultIfEmpty()
                        join ct in DataContext.Categories on v.CategoryId equals ct.Id
                        select new VideoDetail
                        {
                            Id = v.Id,
                            Name = v.Name,
                            Description = v.Description,
                            CreatedValue = v.CreatedOn,
                            Avatar = urlCdn + v.Avatar,
                            UrlLink = v.UrlLink,
                            TimeDuration = v.TimeDuration,
                            View = v.ViewNumber,
                            SubCategory = ct.Name,
                            SubCategoryId = v.CategoryId,
                            Category = ct.Parent == null ? "" : ct.Parent.Name,
                            CategoryId = ct.ParentId.GetValueOrDefault(),
                            CurrentDuration = cw == null ? 0 : cw.CurrentDuration,
                        };
            }
            else
            {
                var categoryParent = (from v in videoCondition
                                      join ct in DataContext.Categories on v.CategoryId equals ct.Id
                                      join cpl in DataContext.CategoryLanguages.Where(cpl => cpl.LanguageId == languageId) on ct.ParentId equals cpl.ParentId
                                      select new CategoryItem
                                      {
                                          Name = cpl.Name,
                                          Id = ct.ParentId.GetValueOrDefault()
                                      }).AsNoTracking().FirstOrDefault();
                query = from v in videoCondition
                        join vl in DataContext.VideoLanguages.Where(vl => vl.LanguageId == languageId) on v.Id equals vl.ParentId
                        join cl in DataContext.CategoryLanguages.Where(cl => cl.LanguageId == languageId) on v.CategoryId equals cl.ParentId into cls
                        from cl in cls.DefaultIfEmpty()
                        join cw in DataContext.CustomerVideoWatcheds.Where(cw => cw.CustomerId == customerId) on v.Id equals cw.VideoId into cws
                        from cw in cws.DefaultIfEmpty()
                        select new VideoDetail
                        {
                            Id = v.Id,
                            Name = vl.Name,
                            Description = vl.Description,
                            CreatedValue = v.CreatedOn,
                            Avatar = string.IsNullOrWhiteSpace(vl.Avatar) ? urlCdn + v.Avatar : urlCdn + vl.Avatar,
                            UrlLink = v.UrlLink,
                            TimeDuration = vl.TimeDuration,
                            View = v.ViewNumber,
                            SubCategory = cl == null ? "" : cl.Name,
                            SubCategoryId = v.CategoryId,
                            Category = categoryParent == null ? "" : categoryParent.Name,
                            CategoryId = categoryParent == null ? null : (int?)categoryParent.Id,
                            CurrentDuration = cw == null ? 0 : cw.CurrentDuration,
                        };
            }
            return query.AsNoTracking().FirstOrDefault();
        }

        public List<VideoItem> GetVideoWatchNext(int videoId, int languageId, int customerId)
        {
            var currentVideo = DataContext.Videos.FirstOrDefault(v => v.IsActive && v.Id == videoId);
            if (currentVideo == null)
            {
                return new List<VideoItem>();
            }

            var listVideoCustom = GetListVideoForCustomer(customerId, languageId);
            listVideoCustom = listVideoCustom.Where(o => o.Id != videoId).ToList();
            var queryListVideoSameCategory = GetListVideoSameCategory(videoId, languageId);
            var listVideoSameCategory = queryListVideoSameCategory.OrderBy(o => Guid.NewGuid()).Take(ConstantValue.LimitVideoSameCategory).AsNoTracking().ToList();
            listVideoSameCategory.AddRange(listVideoCustom);

            return listVideoSameCategory.GroupBy(o => o.Id).Select(grp => grp.First()).Take(ConstantValue.LimitVideoSameCategory).ToList();
        }

        public List<VideoItem> GetVideoMightLove(int videoId, int? page, int languageId)
        {
            var currentVideo = DataContext.Videos.FirstOrDefault(v => v.IsActive && v.Id == videoId);
            if (currentVideo == null)
            {
                return new List<VideoItem>();
            }
            if (page.GetValueOrDefault() < 1)
            {
                page = 1;
            }
            var skip = (page.GetValueOrDefault() - 1) * ConstantValue.LimitVideoMightLove;
            var take = ConstantValue.LimitVideoMightLove;
            var queryListVideoSameCategory = GetListVideoSameCategory(videoId, languageId);
            return queryListVideoSameCategory.OrderByDescending(v => v.Id).Skip(skip).Take(take).AsNoTracking().ToList();
        }

        public int TotalVideoMightLove(int videoId)
        {
            var currentVideo = DataContext.Videos.FirstOrDefault(v => v.IsActive && v.Id == videoId);
            if (currentVideo == null)
            {
                return 0;
            }
            return DataContext.Videos.Count(v => v.IsActive && (v.CategoryId == currentVideo.CategoryId) &&
                                                 (v.Id != currentVideo.Id));
        }

        private IQueryable<VideoItem> GetListVideoSameCategory(int videoId, int languageId)
        {
            var urlCdn = ImageHelper.GetImageUrlCdn();
            var currentVideo = DataContext.Videos.FirstOrDefault(v => v.IsActive && v.Id == videoId);
            var videoCondition = from vsc in DataContext.Videos.Where(vsc => vsc.IsActive && (vsc.CategoryId == currentVideo.CategoryId) &&
                               (vsc.Id != currentVideo.Id))
                                 select vsc;
            IQueryable<VideoItem> query;
            if (languageId == (int)LanguageKey.English)
            {
                query = from vsc in videoCondition
                        select new VideoItem
                        {
                            Id = vsc.Id,
                            Name = vsc.Name,
                            Avatar = urlCdn + vsc.Avatar,
                            UrlLink = vsc.UrlLink,
                            TimeDuration = vsc.TimeDuration,
                            Description = vsc.Description
                        };
            }
            else
            {
                query = from vsc in videoCondition
                        join vl in DataContext.VideoLanguages.Where(vl => vl.LanguageId == languageId) on vsc.Id equals vl.ParentId
                        select new VideoItem
                        {
                            Id = vsc.Id,
                            Name = vl.Name,
                            Avatar = string.IsNullOrWhiteSpace(vl.Avatar) ? urlCdn + vsc.Avatar : urlCdn + vl.Avatar,
                            UrlLink = vsc.UrlLink,
                            TimeDuration = vl.TimeDuration,
                            Description = vl.Description
                        };
            }
            return query;
        }

        public List<VideoSearchItem> Search(string data, int languageId)
        {
            var urlCdn = ImageHelper.GetImageUrlCdn();
            var videoCondition = from v in DataContext.Videos.Where(v => v.IsActive) select v;
            IQueryable<VideoSearchItem> query;

            if (languageId == (int)LanguageKey.English)
            {
                query = (from v in videoCondition
                         where v.Name.ToLower().Contains(data.ToLower())
                         select new VideoSearchItem
                         {
                             Id = v.Id,
                             Name = v.Name,
                             Avatar = urlCdn + v.Avatar,
                             UrlLink = v.UrlLink,
                             Description = v.Description
                         });
            }
            else
            {
                query = (from v in videoCondition
                         join vl in DataContext.VideoLanguages.Where(vl => vl.LanguageId == languageId) on v.Id equals vl.ParentId
                         where vl.Name.ToLower().Contains(data.ToLower())
                         select new VideoSearchItem
                         {
                             Id = v.Id,
                             Name = vl.Name,
                             Avatar = string.IsNullOrWhiteSpace(vl.Avatar) ? urlCdn + v.Avatar : urlCdn + vl.Avatar,
                             UrlLink = v.UrlLink,
                             Description = vl.Description
                         });
            }

            var result = query.AsNoTracking().ToList();
            result = result.OrderBy(o => o.Name).ToList();
            return result;

        }

        public bool UpdateView(List<YoutubeViewResult> listDataNeedUpdate)
        {
            var listId = listDataNeedUpdate.Where(o => o.View.GetValueOrDefault() > 0).Select(o => o.VideoId);
            var listVideo = DataContext.Videos.Where(o => listId.Contains(o.Id));
            foreach (var item in listVideo)
            {
                var itemUpdate = listDataNeedUpdate.FirstOrDefault(o => o.VideoId == item.Id);
                if (itemUpdate != null && itemUpdate.View.GetValueOrDefault() > 0)
                {
                    item.ViewNumber = itemUpdate.View.GetValueOrDefault();
                }
            }
            DataContext.SaveChanges();
            return true;
        }
        public CategoryVideo GetVideoOfCategory(int categoryId, int languageId, int page)
        {
            var category = DataContext.Categories.AsNoTracking().FirstOrDefault(c => c.Id == categoryId && c.IsActive == true);
            IQueryable<Video> videoCondition;
            if (category == null)
            {
                return null;
            }
            //select category or subcategory
            if (!category.Children.Any())
            {
                videoCondition = from v in DataContext.Videos.Where(v => v.CategoryId == categoryId && v.IsActive) select v;
            }
            else
            {
                var listSubCategoryId = new List<int?>();
                foreach (var item in category.Children)
                {
                    listSubCategoryId.Add(item.Id);
                }
                videoCondition = from v in DataContext.Videos.Where(v => listSubCategoryId.Contains(v.CategoryId) && v.IsActive) select v;
            }


            CategoryLanguage categoryLanguage = new CategoryLanguage();
            CategoryLanguage parentCategoryLanguage = new CategoryLanguage();
            var urlCdn = ImageHelper.GetImageUrlCdn();
            var skip = (page - 1) * ConstantValue.LimitVideoCategoty;
            IQueryable<VideoItem> query;
            if (languageId == (int)LanguageKey.English)
            {
                query = (from v in videoCondition
                         select new VideoItem
                         {
                             Id = v.Id,
                             Name = v.Name,
                             Avatar = urlCdn + v.Avatar,
                             UrlLink = v.UrlLink,
                             Description = v.Description
                         });
            }
            else
            {
                query = (from v in videoCondition
                         join vl in DataContext.VideoLanguages.Where(vl => vl.LanguageId == languageId) on v.Id equals vl.ParentId
                         select new VideoItem
                         {
                             Id = v.Id,
                             Name = vl.Name,
                             Avatar = string.IsNullOrWhiteSpace(vl.Avatar) ? urlCdn + v.Avatar : urlCdn + vl.Avatar,
                             UrlLink = v.UrlLink,
                             Description = vl.Description
                         });
                categoryLanguage = DataContext.CategoryLanguages.AsNoTracking().FirstOrDefault(cl => cl.ParentId == categoryId && cl.LanguageId == languageId);
                if (category.ParentId != null)
                {
                    parentCategoryLanguage = DataContext.CategoryLanguages.AsNoTracking().FirstOrDefault(pcl => pcl.ParentId == category.ParentId && pcl.LanguageId == languageId);

                }
            }
            return new CategoryVideo
            {
                CategoryName = languageId == (int)LanguageKey.English ? category.Name : categoryLanguage?.Name,
                CategoryId = category.Id,
                ParentCategory = category.ParentId != null ? (languageId == (int)LanguageKey.English ? category.Parent.Name : parentCategoryLanguage?.Name) : "",
                ParentCategoryId = category.ParentId.GetValueOrDefault(),
                TotalRecord = videoCondition.Count(),
                CurrentPage = page,
                TotalPage = (int)Math.Ceiling((double)videoCondition.Count() / ConstantValue.LimitVideoCategoty),
                ListVideoCategory = query.Skip(skip).Take(ConstantValue.LimitVideoCategoty).AsNoTracking().ToList()
            };
        }
        public VideoItem GetVideoById(int videoId, int languageId)
        {
            var urlCdn = ImageHelper.GetImageUrlCdn();
            var condition = DataContext.Videos.Where(v => v.Id == videoId && v.IsActive);

            IQueryable<VideoItem> query;
            if (languageId == (int)LanguageKey.English)
            {
                query = (from v in condition
                         select new VideoItem
                         {
                             Id = v.Id,
                             Name = v.Name,
                             Avatar = urlCdn + v.Avatar,
                             UrlLink = v.UrlLink,
                             Description = v.Description
                         });
            }
            else
            {
                query = (from v in condition
                         join vl in DataContext.VideoLanguages.Where(vl => vl.LanguageId == languageId) on v.Id equals vl.ParentId
                         select new VideoItem
                         {
                             Id = v.Id,
                             Name = vl.Name,
                             Description = vl.Description,
                             Avatar = string.IsNullOrWhiteSpace(vl.Avatar) ? urlCdn + v.Avatar : urlCdn + vl.Avatar,
                         });
            }

            return query.AsNoTracking().FirstOrDefault();
        }
    }
}