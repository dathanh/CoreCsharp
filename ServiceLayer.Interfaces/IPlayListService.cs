using Framework.DomainModel.Entities;
using Framework.DomainModel.ValueObject;
using System.Collections.Generic;

namespace ServiceLayer.Interfaces
{
    public interface IPlayListService : IMasterFileService<PlayList>
    {
        List<PlayListItem> GetListPlayList(int languageId);
        PlaylistDetail GetPlaylistById(int idPlaylist, int languageId, int currentUserId);
        List<YourPlayListDto> GetYourPlayList(int currentCustomerId);
        List<CustomerPlaylist> GetCustomerPlayList(int currentUserId, AddOrRemoveDataToPlayListDto data);
        bool CreatePlayListForCustomer(PlaylistForCustomer playlistForCustomer, int customerId);
        bool AddRemoveItemToPlayList(AddOrRemoveDataToPlayListDto data, int customerId);
        List<CustomerPlaylistForMenu> GetAllCustomerPlayList(int currentUserId);
        MetaResponse GetSeoInfo(int idPlaylist, int languageId);
    }
}