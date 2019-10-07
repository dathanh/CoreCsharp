using Database.Persistance.Tenants;
using Framework.DomainModel.Entities;
using Repositories.Interfaces;
using System;
using System.Linq;

namespace Repositories
{
    public class EntityFrameworkGridConfigRepository : EntityFrameworkTenantRepositoryBase<GridConfig>, IGridConfigRepository
    {
        public EntityFrameworkGridConfigRepository(ITenantPersistenceService persistenceService) : base(persistenceService)
        {

        }

        public TResult GetGridConfig<TResult>(Func<GridConfig, TResult> selector, int userId, int documentTypeId, string gridInternalName = "")
        {
            var query = GetAll().Where(x => x.UserId == userId
                                            && x.DocumentTypeId == documentTypeId
                                            && (x.GridInternalName == gridInternalName))
                .OrderByDescending(x => x.UserId)
                .ThenByDescending(x => x.GridInternalName);
            var gridConfig = query.Select(selector).FirstOrDefault();

            if (gridConfig == null)
            {
                var defaultConfig = new GridConfig
                {
                    DocumentTypeId = documentTypeId,
                    UserId = userId,
                    GridInternalName = gridInternalName
                };
                return selector(defaultConfig);
            }

            return gridConfig;
        }
    }
}
