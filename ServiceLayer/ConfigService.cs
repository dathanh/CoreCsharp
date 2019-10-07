using Database.Persistance.Tenants;
using Framework.BusinessRule;
using Framework.DomainModel.Entities;
using Repositories.Interfaces;
using ServiceLayer.Interfaces;

namespace ServiceLayer
{
    public class ConfigService : MasterFileService<Config>, IConfigService
    {
        private readonly IConfigRepository _configRepository;
        public ConfigService(ITenantPersistenceService tenantPersistenceService, IConfigRepository configRepository, IBusinessRuleSet<Config> businessRuleSet = null)
            : base(configRepository, configRepository, tenantPersistenceService, businessRuleSet)
        {
            _configRepository = configRepository;
        }

    }
}
