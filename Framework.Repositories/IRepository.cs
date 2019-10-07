using Framework.DomainModel;
namespace Framework.Repositories
{
    public interface IRepository
    {
        Entity GetById(int id);
        void Add(Entity entity);
    }

    public interface IRepository<TEntity> : IRepository where TEntity : Entity
    {
        new TEntity GetById(int id);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);

    }
}
