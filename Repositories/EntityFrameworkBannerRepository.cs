using Database.Persistance.Tenants;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.ValueObject;
using Framework.Utility;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Repositories
{
    public class EntityFrameworkBannerRepository : EntityFrameworkTenantRepositoryBase<Banner>, IBannerRepository
    {
        public EntityFrameworkBannerRepository(ITenantPersistenceService persistenceService)
            : base(persistenceService)
        {
            SearchColumns.Add("Name");
            SearchColumns.Add("UrlLink");
            SearchColumns.Add("Description");
            DisplayColumnForCombobox.Add("Name");
        }

        public override IQueryable<ReadOnlyGridVo> BuildQueryToGetDataForGrid(IQueryInfo queryInfo)
        {
            var query = queryInfo as BannerQueryInfo ?? new BannerQueryInfo();
            var allData = GetAll();
            if (query.Type.GetValueOrDefault() != 0)
            {
                allData = allData.Where(o => o.Type == query.Type);
            }
            return (from s in allData
                    select new BannerGridVo()
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Description = s.Description,
                        UrlLink = s.VideoId == null ? s.UrlLink : s.Video.Name,
                        OrderNumber = s.OrderNumber,
                        Type = s.Type,
                        IsActive = s.IsActive,
                        TimeDuration = s.TimeDuration,
                        IsHideDescription = s.IsHideDescription
                    }).OrderBy(queryInfo.SortString);
        }

        public bool CheckNameLanguageIsExists(int catLanguageId, string name, int languageId)
        {
            return (from o in DataContext.BannerLanguages
                    where o.LanguageId == languageId && o.Id != catLanguageId && o.Name != null &&
                          o.Name.ToLower() == name.ToLower()
                    select o).Any();
        }

        private List<BannerItem> GetListBanner(int languageId, BannerTypeEnum bannerType, int takeItem)
        {
            var urlCdn = ImageHelper.GetImageUrlCdn();
            var bannerWithCondition = DataContext.Banners.Where(o => o.IsActive == true && o.Type == (int)bannerType);
            if (bannerType != BannerTypeEnum.BannerTop)
            {
                bannerWithCondition = bannerWithCondition.OrderBy(o => o.OrderNumber).ThenByDescending(o => o.Id);
            }
            IQueryable<BannerItem> query;
            if (languageId == (int)LanguageKey.English)
            {
                query = (from o in bannerWithCondition
                         join vd in DataContext.Videos on o.VideoId equals vd.Id into vds
                         from vd in vds.DefaultIfEmpty()
                         select new BannerItem
                         {
                             Background = urlCdn + o.Background,
                             Description = o.Description,
                             Id = o.Id,
                             Name = o.Name,
                             TimeDuration = o.TimeDuration,
                             UrlLink = o.UrlLink,
                             VideoFile = urlCdn + o.VideoFile,
                             VideoId = o.VideoId,
                             NameVideo = vd == null ? "" : vd.Name,
                             IsHideDescription = o.IsHideDescription
                         });
            }
            else
            {
                query = (from o in bannerWithCondition
                         join bl in DataContext.BannerLanguages.Where(bl => bl.LanguageId == languageId) on o.Id equals bl.ParentId
                         join vd in DataContext.Videos on o.VideoId equals vd.Id into vds
                         from vd in vds.DefaultIfEmpty()
                         join vdl in DataContext.VideoLanguages.Where(vdl => vdl.LanguageId == languageId) on vd.Id equals vdl.ParentId into vdls
                         from vdl in vdls.DefaultIfEmpty()
                         select new BannerItem
                         {
                             Background = string.IsNullOrWhiteSpace(bl.Background) ? urlCdn + o.Background : urlCdn + bl.Background,
                             Description = bl.Description,
                             Id = o.Id,
                             Name = bl.Name,
                             TimeDuration = bl.TimeDuration,
                             UrlLink = o.UrlLink,
                             VideoFile = urlCdn + o.VideoFile,
                             VideoId = o.VideoId,
                             NameVideo = vdl == null ? "" : vdl.Name,
                             IsHideDescription = o.IsHideDescription
                         });
            }
            query = query.AsNoTracking();
            if (bannerType == BannerTypeEnum.BannerTop)
            {
                query = query.OrderBy(o => Guid.NewGuid());
            }
            return query.Take(takeItem).ToList();
        }

        public List<BannerItem> GetListBannerTop(int languageId)
        {
            return GetListBanner(languageId, BannerTypeEnum.BannerTop, ConstantValue.LimitBannerTop);
        }

        public List<BannerItem> GetListComingSoon(int languageId)
        {
            return GetListBanner(languageId, BannerTypeEnum.ComingSoon, ConstantValue.LimitComingSoon);
        }

        public List<BannerItem> GetLisBannerVideoDetail(int languageId)
        {
            return GetListBanner(languageId, BannerTypeEnum.VideoDetail, ConstantValue.LimitBannerVideoDetail);

        }

        public BannerItem GetListAdvBanner(int languageId)
        {
            return GetListBanner(languageId, BannerTypeEnum.AdvertisementBanner, 1).FirstOrDefault();
        }
    }
}