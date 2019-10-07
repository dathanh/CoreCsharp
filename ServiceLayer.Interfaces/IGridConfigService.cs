using Framework.DomainModel.Entities;
using System;

namespace ServiceLayer.Interfaces
{
    public interface IGridConfigService : IMasterFileService<GridConfig>
    {
        TResult GetGridConfig<TResult>(Func<GridConfig, TResult> selector,
            int? userId,
            int? documentTypeId,
            string gridInternalName = "");
    }
}