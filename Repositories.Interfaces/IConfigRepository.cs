using Framework.DomainModel.Entities;
using Framework.Repositories;

namespace Repositories.Interfaces
{
    public interface IConfigRepository : IRepository<Config>, IQueryableRepository<Config>
    {
    }
}
