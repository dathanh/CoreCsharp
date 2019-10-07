using Database.Persistance.Tenants;
using Framework.DomainModel.Entities;
using Framework.Utility;
using Repositories.Interfaces;
using ServiceLayer.Interfaces.Common;
using System.Collections.Generic;
using System.Linq;

namespace ServiceLayer.Common
{
    public class ConfigSystem : IConfigSystem
    {
        private readonly IConfigRepository _configRepository;
        private IList<Config> _listConfigs = new List<Config>();
        private readonly ITenantPersistenceService _tenantPersistentService;
        public List<Config> ListConfig => _listConfigs.ToList();
        public ConfigSystem(IConfigRepository configRepository, ITenantPersistenceService tenantPersistentService)
        {
            _configRepository = configRepository;
            _tenantPersistentService = tenantPersistentService;
            RefershListData(false);
        }

        public void RefershListData(bool isRefreshTenant = true)
        {
            _listConfigs = new List<Config>();
            if (_listConfigs.Count == 0)
            {
                if (isRefreshTenant)
                {
                    _tenantPersistentService.RefreshTenant();
                }
                _listConfigs = _configRepository.ListAll();
            }
        }

        private string GetBackgroundByKey(string key)
        {
            var urlCdn = ImageHelper.GetImageUrlCdn();
            var item = _listConfigs.FirstOrDefault(o => o.Name == key);
            if (item == null)
            {
                return "";
            }
            return urlCdn + item.Background;
        }

        private string GetVideoByKey(string key)
        {
            var urlCdn = ImageHelper.GetImageUrlCdn();
            var item = _listConfigs.FirstOrDefault(o => o.Name == key);
            if (item == null)
            {
                return "";
            }
            return urlCdn + item.VideoFile;
        }

        public string LoginVideo => GetVideoByKey(ConstantValue.LoginBackground);
        public string ChooseCategoryVideo => GetVideoByKey(ConstantValue.ChooseCategoryBackground);
        public string ChooseSubCategoryVideo => GetVideoByKey(ConstantValue.ChooseSubCategoryBackground);

        public string LoginBackground => GetBackgroundByKey(ConstantValue.LoginBackground);
        public string ChooseCategoryBackground => GetBackgroundByKey(ConstantValue.ChooseCategoryBackground);
        public string ChooseSubCategoryBackground => GetBackgroundByKey(ConstantValue.ChooseSubCategoryBackground);

    }
}
