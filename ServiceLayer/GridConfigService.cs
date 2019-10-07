using Database.Persistance.Tenants;
using Framework.DomainModel.Entities;
using Repositories.Interfaces;
using ServiceLayer.Interfaces;
using System;

namespace ServiceLayer
{
    public class GridConfigService : MasterFileService<GridConfig>, IGridConfigService
    {
        private readonly IGridConfigRepository _gridConfigRepository;
        public GridConfigService(ITenantPersistenceService tenantPersistenceService, IGridConfigRepository gridConfigRepository)
            : base(gridConfigRepository, gridConfigRepository, tenantPersistenceService)
        {
            _gridConfigRepository = gridConfigRepository;
        }

        public TResult GetGridConfig<TResult>(Func<GridConfig, TResult> selector,
            int? userId,
            int? documentTypeId,
            string gridInternalName)
        {
            return _gridConfigRepository.GetGridConfig(selector,
                userId.GetValueOrDefault(),
                documentTypeId.GetValueOrDefault(),
                gridInternalName);
        }
    }
}
