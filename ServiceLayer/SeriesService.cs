using Database.Persistance.Tenants;
using Framework.BusinessRule;
using Framework.DomainModel.Entities;
using Framework.DomainModel.ValueObject;
using Framework.Repositories;
using Repositories.Interfaces;
using ServiceLayer.Interfaces;
using System.Collections.Generic;

namespace ServiceLayer
{
    public class SeriesService : MasterFileService<Series>, ISeriesService
    {
        private readonly ISeriesRepository _seriesRepository;
        private readonly IVideoRepository _videoRepository;
        public SeriesService(ITenantPersistenceService tenantPersistenceService, ISeriesRepository seriesRepository, IVideoRepository videoRepository,
            IBusinessRuleSet<Series> businessRuleSet = null)
            : base(seriesRepository, seriesRepository, tenantPersistenceService, businessRuleSet)
        {
            _seriesRepository = seriesRepository;
            _videoRepository = videoRepository;
        }

        public int AddSeries(Series entity, List<int> listVideoId)
        {
            ValidateBusinessRules(entity);
            Repository.Add(entity);
            var listVideo = _videoRepository.Get(o => listVideoId.Contains(o.Id));
            foreach (var video in listVideo)
            {
                video.SeriesId = entity.Id;
            }

            if (Repository is IEntityFrameworkRepository<Series>)
            {
                (Repository as IEntityFrameworkRepository<Series>).Commit();
            }

            return entity.Id;
        }

        public SeriesItem GetSeriesItem(int languageId)
        {
            return _seriesRepository.GetSeriesItem(languageId);
        }

        public byte[] UpdateSeries(Series entity, List<int> listVideoId)
        {
            ValidateBusinessRules(entity);

            Repository.Update(entity);
            var listVideoExists = _videoRepository.Get(o => o.SeriesId == entity.Id);
            foreach (var video in listVideoExists)
            {
                video.SeriesId = null;
            }
            var listVideo = _videoRepository.Get(o => listVideoId.Contains(o.Id));
            foreach (var video in listVideo)
            {
                video.SeriesId = entity.Id;
            }

            if (Repository is IEntityFrameworkRepository<Series>)
            {
                (Repository as IEntityFrameworkRepository<Series>).Commit();
            }
            return entity.LastModified;
        }
    }
}
