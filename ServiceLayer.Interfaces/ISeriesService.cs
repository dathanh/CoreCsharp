using Framework.DomainModel.Entities;
using Framework.DomainModel.ValueObject;
using System.Collections.Generic;

namespace ServiceLayer.Interfaces
{
    public interface ISeriesService : IMasterFileService<Series>
    {
        SeriesItem GetSeriesItem(int languageId);
        int AddSeries(Series entity, List<int> listVideoId);
        byte[] UpdateSeries(Series entity, List<int> listVideoId);
    }
}
