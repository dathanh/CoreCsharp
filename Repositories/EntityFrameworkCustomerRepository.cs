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
    public class EntityFrameworkCustomerRepository : EntityFrameworkTenantRepositoryBase<Customer>, ICustomerRepository
    {
        public EntityFrameworkCustomerRepository(ITenantPersistenceService persistenceService)
            : base(persistenceService)
        {
            SearchColumns.Add("UserName");
            SearchColumns.Add("FullName");
            SearchColumns.Add("Email");
            DisplayColumnForCombobox.Add("UserName");
        }
        public override IQueryable<ReadOnlyGridVo> BuildQueryToGetDataForGrid(IQueryInfo queryInfo)
        {
            var query = queryInfo as CustomerQueryInfo ?? new CustomerQueryInfo();
            var allItem = GetAll();
            if (query.Type.GetValueOrDefault() != 0)
            {
                if (query.Type == 1)
                {
                    allItem = allItem.Where(o => !o.IsAccountGoogle.GetValueOrDefault() && !o.IsAccountFacebook.GetValueOrDefault());
                }
                else if (query.Type == 2)
                {
                    allItem = allItem.Where(o => o.IsAccountGoogle.GetValueOrDefault());
                }
                else if (query.Type == 3)
                {
                    allItem = allItem.Where(o => o.IsAccountFacebook.GetValueOrDefault());
                }
            }
            if (query.RegisterStartDate.GetValueOrDefault() != DateTime.MinValue)
            {
                allItem = allItem.Where(o => o.CreatedOn >= query.RegisterStartDate);
            }
            if (query.RegisterEndDate.GetValueOrDefault() != DateTime.MinValue)
            {
                query.RegisterEndDate = query.RegisterEndDate.GetValueOrDefault().AddDays(1).AddMilliseconds(-1);
                allItem = allItem.Where(o => o.CreatedOn <= query.RegisterEndDate);
            }
            var queryResult = (from cus in allItem
                               select new CustomerGridVo
                               {
                                   Id = cus.Id,
                                   UserName = cus.UserName,
                                   Dob = cus.Dob,
                                   FullName = cus.FullName,
                                   Email = cus.Email,
                                   Phone = cus.Phone,
                                   IsAccountFacebook = cus.IsAccountFacebook,
                                   IsAccountGoogle = cus.IsAccountGoogle,
                                   CreatedDate = cus.CreatedOn
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