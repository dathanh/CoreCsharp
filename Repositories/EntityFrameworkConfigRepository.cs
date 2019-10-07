using Database.Persistance.Tenants;
using Framework.DomainModel.Entities;
using Framework.DomainModel.ValueObject;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Repositories
{
    public class EntityFrameworkConfigRepository : EntityFrameworkTenantRepositoryBase<Config>, IConfigRepository
    {
        public EntityFrameworkConfigRepository(ITenantPersistenceService persistenceService)
            : base(persistenceService)
        {
            SearchColumns.Add("Name");
            SearchColumns.Add("Value");
            SearchColumns.Add("Description");
            DisplayColumnForCombobox.Add("Name");
        }
        public override IQueryable<ReadOnlyGridVo> BuildQueryToGetDataForGrid(IQueryInfo queryInfo)
        {
            var queryResult = (from conf in GetAll()
                               select new ConfigGridVo
                               {
                                   //Value = conf.Value,
                                   Name = conf.Name,
                                   Description = conf.Description,
                                   Id = conf.Id,
                                   VideoName = conf.VideoName ?? ""

                               }).OrderBy(queryInfo.SortString);

            return queryResult;
        }


        protected override void BuildSortExpression(IQueryInfo queryInfo)
        {
            if (queryInfo.Sort == null || queryInfo.Sort.Count == 0)
            {
                queryInfo.Sort = new List<Sort> { new Sort { Field = "Id", Dir = "desc" } };
            }
            queryInfo.Sort.ForEach(x =>
            {
                x.Field = String.Format("{0}", x.Field);
            });
        }
    }
}
