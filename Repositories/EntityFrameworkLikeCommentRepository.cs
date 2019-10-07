using Database.Persistance.Tenants;
using Framework.DomainModel.Entities;
using Repositories.Interfaces;

namespace Repositories
{
    public class EntityFrameworkLikeCommentRepository : EntityFrameworkTenantRepositoryBase<LikeComment>, ILikeCommentRepository
    {
        public EntityFrameworkLikeCommentRepository(ITenantPersistenceService persistenceService)
            : base(persistenceService)
        {

        }
    }
}