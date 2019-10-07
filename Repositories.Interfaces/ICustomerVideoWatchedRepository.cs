using Framework.DomainModel.Entities;
using Framework.Repositories;

namespace Repositories.Interfaces
{
    public interface ICustomerVideoWatchedRepository : IEntityFrameworkRepository<CustomerVideoWatched>, IQueryableRepository<CustomerVideoWatched>
    {

    }
}