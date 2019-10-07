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
    public class EntityFrameworkPlayListRepository : EntityFrameworkTenantRepositoryBase<PlayList>, IPlayListRepository
    {
        public EntityFrameworkPlayListRepository(ITenantPersistenceService persistenceService)
            : base(persistenceService)
        {
            SearchColumns.Add("Name");
            SearchColumns.Add("Description");
            DisplayColumnForCombobox.Add("Name");
        }

        public override IQueryable<ReadOnlyGridVo> BuildQueryToGetDataForGrid(IQueryInfo queryInfo)
        {

            return (from s in GetAll().Where(s => s.OwnerCustomerId == null)
                    select new PlayListGridVo()
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Description = s.Description,
                        IsActive = s.IsActive,
                        OrderNumber = s.OrderNumber
                    }).OrderBy(queryInfo.SortString);
        }

        public bool CheckNameLanguageIsExists(int catLanguageId, string name, int languageId)
        {
            return (from o in DataContext.PlayListLanguages
                    where o.LanguageId == languageId && o.Id != catLanguageId && o.Name != null &&
                          o.Name.ToLower() == name.ToLower()
                    select o).Any();
        }

        public List<PlayListItem> GetListPlayList(int languageId)
        {
            var urlCdn = ImageHelper.GetImageUrlCdn();
            var queryAll = DataContext.PlayLists.Where(pl => pl.IsActive && pl.OwnerCustomerId == null && pl.VideoPlayLists.Any());
            IQueryable<PlayListItem> playList;
            if (languageId == (int)LanguageKey.English)
            {
                playList = queryAll.Select(pl => new PlayListItem
                {
                    Id = pl.Id,
                    Name = pl.Name,
                    Description = pl.Description,
                    TotalVideos = pl.VideoPlayLists.Count(vp => vp.VideoId != null),
                    Background = urlCdn + pl.Background,
                    ListVideoItems = (from vp in pl.VideoPlayLists.Where(vp => vp.VideoId != null)
                                      join video in DataContext.Videos on vp.VideoId equals video.Id
                                      orderby vp.Id
                                      select new VideoPlayListItem
                                      {
                                          Id = video.Id,
                                          Name = video.Name,
                                          UrlLink = video.UrlLink,
                                          Avatar = urlCdn + video.Avatar,
                                      }).Take(ConstantValue.LimitVideoPlaylist).ToList()
                });
            }
            else
            {
                playList = (from p in queryAll
                            join pl in DataContext.PlayListLanguages.Where(pl => pl.LanguageId == languageId) on p.Id equals pl.ParentId
                            select new PlayListItem
                            {
                                Id = p.Id,
                                Name = pl.Name,
                                Description = pl.Description,
                                TotalVideos = p.VideoPlayLists.Count(vp => vp.VideoId != null),
                                Background = string.IsNullOrWhiteSpace(pl.Background) ? urlCdn + p.Background : urlCdn + pl.Background,
                                ListVideoItems = (from vp in p.VideoPlayLists.Where(vp => vp.VideoId != null)
                                                  join video in DataContext.Videos on vp.VideoId equals video.Id
                                                  join vpl in DataContext.VideoLanguages.Where(vpl => vpl.LanguageId == languageId) on video.Id equals vpl.ParentId
                                                  orderby vp.Id
                                                  select new VideoPlayListItem
                                                  {
                                                      Id = video.Id,
                                                      Name = vpl.Name,
                                                      UrlLink = video.UrlLink,
                                                      Avatar = string.IsNullOrWhiteSpace(vpl.Avatar) ? urlCdn + video.Avatar : urlCdn + vpl.Avatar
                                                  }
                                                 ).Take(ConstantValue.LimitVideoPlaylist).ToList()
                            });
            }

            playList = playList.OrderBy(p => Guid.NewGuid());
            return playList.Take(ConstantValue.LimitPlaylist).AsNoTracking().ToList();
        }

        public List<CustomerPlaylist> GetCustomerPlayList(int currentUserId, AddOrRemoveDataToPlayListDto data)
        {
            var listVideoPlayListId = new List<int?>();
            if (data.Type == 1)
            {
                listVideoPlayListId = DataContext.VideoPlayLists.Where(v => v.VideoId == data.VideoId).Select(v => v.PlayListId).ToList();
            }
            else if (data.Type == 2)
            {
                listVideoPlayListId = DataContext.VideoPlayLists.Where(v => v.BannerId == data.BannerId).Select(u => u.PlayListId).ToList();
            }
            return DataContext.PlayLists.Where(p => p.OwnerCustomerId == currentUserId && p.IsActive).Select(p => new CustomerPlaylist
            {
                Id = p.Id,
                Name = p.Name,
                Exists = listVideoPlayListId.Contains(p.Id)
            }).OrderByDescending(v => v.Id).AsNoTracking().ToList();

        }

        public PlaylistDetail GetPlaylistById(int idPlaylist, int languageId, int currentUserId)
        {
            var urlCdn = ImageHelper.GetImageUrlCdn();
            var query = DataContext.PlayLists.Where(pl => pl.IsActive && pl.Id == idPlaylist && (pl.OwnerCustomerId == currentUserId || pl.OwnerCustomerId == null));

            if (languageId == (int)LanguageKey.English)
            {
                return query.Select(pl => new PlaylistDetail
                {
                    Id = pl.Id,
                    Name = pl.Name,
                    Description = pl.Description,
                    TotalVideos = pl.VideoPlayLists.Count,
                    Background = urlCdn + pl.Background,
                    ListVideoItemDetails = (from vp in pl.VideoPlayLists
                                            join video in DataContext.Videos on vp.VideoId equals video.Id into videos
                                            from video in videos.DefaultIfEmpty()
                                            join cw in DataContext.CustomerVideoWatcheds.Where(cw => cw.CustomerId == currentUserId) on video.Id equals cw.VideoId into cws
                                            from cw in cws.DefaultIfEmpty()
                                            select new VideoPlayListItemDetail
                                            {
                                                Id = vp.Id,
                                                VideoId = video.Id,
                                                Name = video != null ? video.Name : "",
                                                UrlLink = video != null ? video.UrlLink : "",
                                                Avatar = video != null ? (urlCdn + video.Avatar) : "",
                                                Description = video != null ? video.Description : "",
                                                CurrentDuration = cw.CurrentDuration,
                                            }).OrderBy(vp => vp.Id).ToList()
                }).AsNoTracking().FirstOrDefault();
            }

            return (from p in query
                    join pl in DataContext.PlayListLanguages.Where(pl => pl.LanguageId == languageId) on p.Id equals pl.ParentId into pls
                    from pl in pls.DefaultIfEmpty()
                    select new PlaylistDetail
                    {
                        Id = p.Id,
                        Name = pl == null ? p.Name : pl.Name,
                        Description = pl == null ? p.Description : pl.Description,
                        TotalVideos = p.VideoPlayLists.Count,
                        Background = string.IsNullOrWhiteSpace(pl.Background) ? urlCdn + p.Background : urlCdn + pl.Background,
                        ListVideoItemDetails = (from vp in p.VideoPlayLists
                                                join video in DataContext.Videos on vp.VideoId equals video.Id into videos
                                                from video in videos.DefaultIfEmpty()
                                                join cw in DataContext.CustomerVideoWatcheds.Where(cw => cw.CustomerId == currentUserId) on video.Id equals cw.VideoId into cws
                                                from cw in cws.DefaultIfEmpty()
                                                join vl in DataContext.VideoLanguages.Where(vl => vl.LanguageId == languageId) on video.Id equals vl.ParentId into vls
                                                from vl in vls.DefaultIfEmpty()
                                                select new VideoPlayListItemDetail
                                                {
                                                    Id = vp.Id,
                                                    VideoId = video.Id,
                                                    Name = vl != null ? vl.Name : "",
                                                    UrlLink = video != null ? video.UrlLink : "",
                                                    Avatar = video != null ? (string.IsNullOrWhiteSpace(vl.Avatar) ? urlCdn + video.Avatar : urlCdn + vl.Avatar) : "",
                                                    Description = vl != null ? vl.Description : "",
                                                    CurrentDuration = cw.CurrentDuration
                                                }).OrderBy(vp => vp.Id).ToList()

                    }).AsNoTracking().FirstOrDefault();

        }

        public List<CustomerPlaylistForMenu> GetAllCustomerPlayList(int currentUserId)
        {
            return DataContext.PlayLists.Where(p => p.OwnerCustomerId == currentUserId && p.IsActive).Select(p => new CustomerPlaylistForMenu
            {
                Id = p.Id,
                Name = p.Name,
            }).OrderBy(p => p.Name).AsNoTracking().ToList();
        }
        public List<YourPlayListDto> GetYourPlayList(int currentCustomerId)
        {
            var urlCdn = ImageHelper.GetImageUrlCdn();
            return DataContext.PlayLists.Where(p => p.OwnerCustomerId == currentCustomerId && p.IsActive).Select(p => new YourPlayListDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                TotalAll = p.VideoPlayLists.Count,
                BackgroundVideo = p.VideoPlayLists.FirstOrDefault(o => o.VideoId != null) == null ? "" : urlCdn + p.VideoPlayLists.First(o => o.VideoId != null).Video.Avatar,
                BackgroundUrl = p.VideoPlayLists.FirstOrDefault(o => o.BannerId != null) == null ? "" : urlCdn + p.VideoPlayLists.First(o => o.BannerId != null).Banner.Background,
            }).OrderByDescending(p => p.Id).AsNoTracking().ToList();
        }
        public PlayListItem GetPlaylistById(int playlistId, int languageId)
        {
            var urlCdn = ImageHelper.GetImageUrlCdn();
            var queryAll = DataContext.PlayLists.Where(pl => pl.IsActive && pl.OwnerCustomerId == null && pl.Id == playlistId);
            IQueryable<PlayListItem> playList;
            if (languageId == (int)LanguageKey.English)
            {
                playList = (from p in queryAll
                            select new PlayListItem
                            {
                                Id = p.Id,
                                Name = p.Name,
                                Description = p.Description,
                                Background = urlCdn + p.Background,
                            });
            }
            else
            {
                playList = (from p in queryAll
                            join pl in DataContext.PlayListLanguages.Where(pl => pl.LanguageId == languageId) on p.Id equals pl.ParentId
                            select new PlayListItem
                            {
                                Id = p.Id,
                                Name = pl.Name,
                                Description = pl.Description,
                                Background = string.IsNullOrWhiteSpace(pl.Background) ? urlCdn + p.Background : urlCdn + pl.Background,
                            });
            }

            return playList.AsNoTracking().FirstOrDefault();
        }
    }
}
