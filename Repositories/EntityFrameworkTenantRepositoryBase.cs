using Database.Persistance.Tenants;
using Framework.DomainModel;

namespace Repositories
{
    public class EntityFrameworkTenantRepositoryBase<TEntity> :
        EntityFrameworkRepositoryBase<ITenantPersistenceService, ITenantWorkspace, TEntity>
        where TEntity : Entity
    {
        public virtual ITenantPersistenceService TenantPersistenceService { get; protected set; }

        public TenantDataContext DataContext => PersistenceService.CurrentWorkspace.Context;

        public EntityFrameworkTenantRepositoryBase(ITenantPersistenceService persistenceService)
            : base(persistenceService, x => x.Context)
        {
            TenantPersistenceService = persistenceService;
        }
    }
}