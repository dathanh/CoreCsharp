using Framework.DomainModel;
using ServiceLayer.Authorization;
using ServiceLayer.Interfaces;
using ProjectName.Models.Base;

namespace ProjectName.Controllers.Base
{
    public abstract class ApplicationControllerBase : ApplicationControllerGeneric<Entity, MasterfileViewModelBase<Entity>>
    {
        //
        // GET: /ApplicationControllerBase/

        protected ApplicationControllerBase(IOperationAuthorization operationAuthorization = null)
            : base(null, operationAuthorization)
        {
        }

        protected ApplicationControllerBase(IMasterFileService<Entity> masterfileService,
             IOperationAuthorization operationAuthorization = null)
            : base(masterfileService, operationAuthorization)
        {
        }

    }
}
