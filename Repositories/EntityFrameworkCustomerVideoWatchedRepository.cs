using Database.Persistance.Tenants;
using Framework.DomainModel.Entities;
using Repositories.Interfaces;

namespace Repositories
{
    public class EntityFrameworkCustomerVideoWatchedRepository : EntityFrameworkTenantRepositoryBase<CustomerVideoWatched>, ICustomerVideoWatchedRepository
    {
        public EntityFrameworkCustomerVideoWatchedRepository(ITenantPersistenceService persistenceService)
            : base(persistenceService)
        {

        }
    }
}