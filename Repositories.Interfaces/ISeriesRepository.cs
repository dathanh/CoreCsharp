using Framework.DomainModel.Entities;
using Framework.DomainModel.ValueObject;
using Framework.Repositories;

namespace Repositories.Interfaces
{
    public interface ISeriesRepository : IEntityFrameworkRepository<Series>, IQueryableRepository<Series>
    {
        bool CheckNameLanguageIsExists(int catLanguageId, string name, int languageId);
        SeriesItem GetSeriesItem(int languageId);
    }
}
