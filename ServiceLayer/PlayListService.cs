using Database.Persistance.Tenants;
using Framework.BusinessRule;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.ValueObject;
using Framework.Exceptions;
using Repositories.Interfaces;
using ServiceLayer.Interfaces;
using ServiceLayer.Translate;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ServiceLayer
{
    public class PlayListService : MasterFileService<PlayList>, IPlayListService
    {
        private readonly IPlayListRepository _playListRepository;
        private readonly IFrontEndMessageLookup _frontEndMessageLookup;
        public PlayListService(ITenantPersistenceService tenantPersistenceService, IPlayListRepository playListRepository,
             IFrontEndMessageLookup frontEndMessageLookup = null, IBusinessRuleSet<PlayList> businessRuleSet = null)
            : base(playListRepository, playListRepository, tenantPersistenceService, businessRuleSet)
        {
            _playListRepository = playListRepository;
            _frontEndMessageLookup = frontEndMessageLookup;
        }

        public List<PlayListItem> GetListPlayList(int languageId)
        {
            return _playListRepository.GetListPlayList(languageId);
        }

        public PlaylistDetail GetPlaylistById(int idPlaylist, int languageId, int currentUserId)
        {
            return _playListRepository.GetPlaylistById(idPlaylist, languageId, currentUserId);
        }
        public List<CustomerPlaylist> GetCustomerPlayList(int currentUserId, AddOrRemoveDataToPlayListDto data)
        {
            return _playListRepository.GetCustomerPlayList(currentUserId, data);
        }
        public List<CustomerPlaylistForMenu> GetAllCustomerPlayList(int currentUserId)
        {
            return _playListRepository.GetAllCustomerPlayList(currentUserId);
        }

        public bool CreatePlayListForCustomer(PlaylistForCustomer playlistForCustomer, int customerId)
        {
            var failed = false;
            var validationResult = new List<ValidationResult>();
            if (string.IsNullOrWhiteSpace(playlistForCustomer.Name))
            {
                var mess = _frontEndMessageLookup.GetMessage("PlaylistNameRequired");
                validationResult.Add(new ValidationResult(mess));
                failed = true;
            }
            else if (_playListRepository.CheckExist(o => o.Name.ToLower() == playlistForCustomer.Name.ToLower() && o.OwnerCustomerId == customerId))
            {
                var mess = _frontEndMessageLookup.GetMessage("PlaylistNameIsExists");
                validationResult.Add(new ValidationResult(mess));
                failed = true;
            }

            if (failed)
            {
                var result = new BusinessRuleResult(true, "", "RegisterCustomer", 0, null, "RegisterCustomerRule") { ValidationResults = validationResult };
                throw new BusinessRuleException("BusinessGenericErrorMessageKey", new[] { result });
            }

            var playlist = new PlayList
            {
                Name = playlistForCustomer.Name,
                Description = playlistForCustomer.Description,
                OwnerCustomerId = customerId,
                IsActive = true,
                OrderNumber = 0
            };
            _playListRepository.Add(playlist);
            _playListRepository.Commit();
            return true;
        }

        public bool AddRemoveItemToPlayList(AddOrRemoveDataToPlayListDto data, int customerId)
        {
            // Validate
            var failed = false;
            var validationResult = new List<ValidationResult>();
            var playList = _playListRepository.GetById(data.PlayListId);

            if (playList == null)
            {
                var mess = _frontEndMessageLookup.GetMessage("PlayListNotNull");
                validationResult.Add(new ValidationResult(mess));
                failed = true;
            }
            // Check videoId must different 0
            if (data.VideoId == 0 && data.Type == 1)
            {
                var mess = _frontEndMessageLookup.GetMessage("VideoNotExists");
                validationResult.Add(new ValidationResult(mess));
                failed = true;
            }
            // Check UrlLink must different null
            if (data.BannerId == 0 && data.Type == 2)
            {
                var mess = _frontEndMessageLookup.GetMessage("BannerNotExists");
                validationResult.Add(new ValidationResult(mess));
                failed = true;
            }

            if (failed)
            {
                var result = new BusinessRuleResult(true, "", "SaveChangePassword", 0, null, "SaveChangePasswordRule") { ValidationResults = validationResult };
                throw new BusinessRuleException("BusinessGenericErrorMessageKey", new[] { result });
            }

            if (data.Type == (int)AddRemoveItemToPlayListType.Video)
            {
                var videoExist = playList.VideoPlayLists.Where(o => o.VideoId == data.VideoId);
                var videoPlayLists = videoExist as VideoPlayList[] ?? videoExist.ToArray();

                if (videoPlayLists.Any())
                {
                    foreach (var item in videoPlayLists)
                    {
                        item.IsDeleted = true;
                    }
                }
                else
                {
                    playList.VideoPlayLists.Add(new VideoPlayList
                    {
                        VideoId = data.VideoId
                    });
                }
            }
            else if (data.Type == (int)AddRemoveItemToPlayListType.Banner)
            {
                var videoExist = playList.VideoPlayLists.Where(o => o.BannerId == data.BannerId);
                var urlLinkWithPlayLists = videoExist as VideoPlayList[] ?? videoExist.ToArray();

                if (urlLinkWithPlayLists.Any())
                {
                    foreach (var item in urlLinkWithPlayLists)
                    {
                        item.IsDeleted = true;
                    }
                }
                else
                {
                    playList.VideoPlayLists.Add(new VideoPlayList
                    {
                        BannerId = data.BannerId
                    });
                }
            }

            _playListRepository.Commit();
            return true;
        }

        public List<YourPlayListDto> GetYourPlayList(int currentCustomerId)
        {
            return _playListRepository.GetYourPlayList(currentCustomerId);
        }
        public MetaResponse GetSeoInfo(int playlistId, int languageId)
        {
            var playlistInfo = _playListRepository.GetPlaylistById(playlistId, languageId);
            if (playlistInfo != null)
            {
                return new MetaResponse()
                {
                    Title = playlistInfo.Name,
                    Description = playlistInfo.Description,
                    Image = playlistInfo.Background,
                };
            }
            return new MetaResponse();
        }
    }
}