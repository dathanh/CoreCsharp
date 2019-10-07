using Database.Persistance.Tenants;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.ValueObject;
using Framework.Utility;
using Repositories.Interfaces;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Repositories
{
    public class EntityFrameworkSeriesRepository : EntityFrameworkTenantRepositoryBase<Series>, ISeriesRepository
    {
        public EntityFrameworkSeriesRepository(ITenantPersistenceService persistenceService)
            : base(persistenceService)
        {
            SearchColumns.Add("Name");
            SearchColumns.Add("Description");
            SearchColumns.Add("OrderNumber");
            DisplayColumnForCombobox.Add("Name");
        }

        public override IQueryable<ReadOnlyGridVo> BuildQueryToGetDataForGrid(IQueryInfo queryInfo)
        {

            return (from s in GetAll()
                    select new SeriesGridVo()
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Description = s.Description,
                        IsActive = s.IsActive,
                        OrderNumber = s.OrderNumber,
                        Background = s.Background
                    }).OrderBy(queryInfo.SortString);
        }

        public bool CheckNameLanguageIsExists(int catLanguageId, string name, int languageId)
        {
            return (from o in DataContext.SeriesLanguages
                    where o.LanguageId == languageId && o.Id != catLanguageId && o.Name != null &&
                          o.Name.ToLower() == name.ToLower()
                    select o).Any();
        }

        public SeriesItem GetSeriesItem(int languageId)
        {
            var urlCdn = ImageHelper.GetImageUrlCdn();
            var seriesConditionData = from s in DataContext.Series.Where(s => s.IsActive && s.Videos.Any()) select s;
            IQueryable<SeriesItem> query;
            if (languageId == (int)LanguageKey.English)
            {
                query = from s in seriesConditionData
                        select new SeriesItem
                        {
                            Id = s.Id,
                            Name = s.Name,
                            Description = s.Description,
                            ListVideoItems = DataContext.Videos.Where(v => v.IsActive && v.SeriesId == s.Id).Select(v => new VideoItem
                            {
                                Id = v.Id,
                                Name = v.Name,
                                Avatar = urlCdn + v.Avatar,
                                UrlLink = v.UrlLink,
                                TimeDuration = v.TimeDuration
                            }).ToList(),
                            Background = urlCdn + s.Background
                        };
            }
            else
            {
                query = from s in seriesConditionData
                        join sl in DataContext.SeriesLanguages.Where(sl => sl.LanguageId == languageId) on s.Id equals sl
                            .ParentId
                        select new SeriesItem
                        {
                            Id = s.Id,
                            Name = sl.Name,
                            Description = sl.Description,
                            ListVideoItems = (from v in DataContext.Videos.Where(v => v.IsActive && v.SeriesId == s.Id)
                                              join vl in DataContext.VideoLanguages.Where(vl => vl.LanguageId == languageId) on v.Id
                                                  equals vl.ParentId
                                              select new VideoItem
                                              {
                                                  Id = v.Id,
                                                  Name = vl.Name,
                                                  Avatar = string.IsNullOrWhiteSpace(vl.Avatar) ? urlCdn + v.Avatar : urlCdn + vl.Avatar,
                                                  UrlLink = v.UrlLink,
                                                  TimeDuration = vl.TimeDuration
                                              }).ToList(),
                            Background = string.IsNullOrWhiteSpace(sl.Background) ? urlCdn + s.Background : urlCdn + sl.Background
                        };


            }

            return query.OrderBy(s => Guid.NewGuid()).AsNoTracking().FirstOrDefault();
        }
    }
}
