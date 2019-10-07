using Database.Persistance.Tenants;
using Framework.BusinessRule;
using Framework.DomainModel.Entities;
using Framework.DomainModel.ValueObject;
using Repositories.Interfaces;
using ServiceLayer.Interfaces;
using System.Collections.Generic;

namespace ServiceLayer
{
    public class BannerService : MasterFileService<Banner>, IBannerService
    {
        private readonly IBannerRepository _bannerRepository;

        public BannerService(ITenantPersistenceService tenantPersistenceService, IBannerRepository bannerRepository,
            IBusinessRuleSet<Banner> businessRuleSet = null)
            : base(bannerRepository, bannerRepository, tenantPersistenceService, businessRuleSet)
        {
            _bannerRepository = bannerRepository;
        }

        public List<BannerItem> GetListBannerTop(int languageId)
        {
            return _bannerRepository.GetListBannerTop(languageId);
        }

        public List<BannerItem> GetListComingSoon(int languageId)
        {
            return _bannerRepository.GetListComingSoon(languageId);
        }

        public List<BannerItem> GetLisBannerVideoDetail(int languageId)
        {
            return _bannerRepository.GetLisBannerVideoDetail(languageId);
        }

        public BannerItem GetListAdvBanner(int languageId)
        {
            return _bannerRepository.GetListAdvBanner(languageId);
        }
    }
}