using Framework.DomainModel.Entities;
using Framework.DomainModel.ValueObject;
using System.Collections.Generic;

namespace ServiceLayer.Interfaces
{
    public interface IBannerService : IMasterFileService<Banner>
    {
        List<BannerItem> GetListBannerTop(int languageId);
        List<BannerItem> GetListComingSoon(int languageId);
        List<BannerItem> GetLisBannerVideoDetail(int languageId);
        BannerItem GetListAdvBanner(int languageId);

    }
}