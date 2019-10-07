using Framework.DomainModel;

namespace Framework.Repositories
{
    public interface IMongoDbRepository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
    }
}
