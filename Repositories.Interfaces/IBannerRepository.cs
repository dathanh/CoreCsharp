using Framework.DomainModel.Entities;
using Framework.DomainModel.ValueObject;
using Framework.Repositories;
using System.Collections.Generic;

namespace Repositories.Interfaces
{
    public interface IBannerRepository : IEntityFrameworkRepository<Banner>, IQueryableRepository<Banner>
    {
        bool CheckNameLanguageIsExists(int catLanguageId, string name, int languageId);
        List<BannerItem> GetListBannerTop(int languageId);
        List<BannerItem> GetListComingSoon(int languageId);
        List<BannerItem> GetLisBannerVideoDetail(int languageId);
        BannerItem GetListAdvBanner(int languageId);
    }
}