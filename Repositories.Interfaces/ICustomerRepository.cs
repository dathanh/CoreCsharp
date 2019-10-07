using Framework.DomainModel.Entities;
using Framework.Repositories;

namespace Repositories.Interfaces
{
    public interface ICustomerRepository : IEntityFrameworkRepository<Customer>, IQueryableRepository<Customer>
    {
    }
}