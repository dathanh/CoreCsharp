using Framework.DomainModel.Entities;
using Framework.DomainModel.ValueObject;
using Framework.Repositories;
using System.Collections.Generic;

namespace Repositories.Interfaces
{
    public interface IPlayListRepository : IEntityFrameworkRepository<PlayList>, IQueryableRepository<PlayList>
    {
        bool CheckNameLanguageIsExists(int videoLanguageId, string name, int languageId);
        List<PlayListItem> GetListPlayList(int languageId);
        PlaylistDetail GetPlaylistById(int idPlaylist, int languageId, int currentUserId);
        List<CustomerPlaylist> GetCustomerPlayList(int currentUserId, AddOrRemoveDataToPlayListDto data);
        List<CustomerPlaylistForMenu> GetAllCustomerPlayList(int currentCustomerId);
        List<YourPlayListDto> GetYourPlayList(int currentCustomerId);
        PlayListItem GetPlaylistById(int playlistId, int languageId);

    }
}