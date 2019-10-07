using Database.Persistance;
using Framework.DomainModel;
using Framework.DomainModel.ValueObject;
using Framework.QueryEngine;
using Framework.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;

namespace Repositories
{
    public class EntityFrameworkRepositoryBase<TPersistenceService, TWorkspace, TEntity> : IEntityFrameworkRepository<TEntity>, IQueryableRepository<TEntity>
        where TEntity : Entity
        where TPersistenceService : IPersistenceService<TWorkspace>
        where TWorkspace : IWorkspace
    {
        public EntityFrameworkRepositoryBase(TPersistenceService persistenceService,
            Func<TWorkspace, DbContext> contextSelector)
        {
            SearchColumns = new Collection<string>();
            DisplayColumnForCombobox = new Collection<string>();
            PersistenceService = persistenceService;
            ContextSelector = contextSelector;
            SetSelector = x => contextSelector(x).Set<TEntity>();
        }

        public Func<TWorkspace, DbContext> ContextSelector { get; }
        public Func<TWorkspace, DbSet<TEntity>> SetSelector { get; }
        public TPersistenceService PersistenceService { get; set; }

        /// <summary>
        /// List column in grid which is used for search
        /// </summary>
        public Collection<string> SearchColumns { get; set; }

        public Collection<string> DisplayColumnForCombobox { get; set; }

        /// <summary>
        /// Get entity by Id
        /// </summary>
        /// <param name="id">Identify key of entity</param>
        /// <returns>Entity found</returns>
        Entity IRepository.GetById(int id)
        {
            return GetById(id);
        }

        /// <summary>
        /// Add entity
        /// </summary>
        /// <param name="entity">Entity which will be added to the system</param>
        public virtual void Add(TEntity entity)
        {
            var set = SetSelector(PersistenceService.CurrentWorkspace);
            set.Add(entity);
        }

        /// <summary>
        /// Remove entity
        /// </summary>
        /// <param name="entity">Entity which will be removed to the system</param>
        public virtual void Remove(TEntity entity)
        {
            var set = SetSelector(PersistenceService.CurrentWorkspace);
            set.Remove(entity);
        }

        /// <summary>
        /// Attach entity to db context
        /// </summary>
        /// <param name="entity">Entity which will be attach to the db context</param>
        public virtual void Attach(TEntity entity)
        {
            var set = SetSelector(PersistenceService.CurrentWorkspace);
            set.Attach(entity);
        }

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity which will be updated to the system</param>
        public virtual void Update(TEntity entity)
        {
            InsertOrUpdate(entity);
        }

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity which will be deleted to the system</param>
        public virtual void Delete(TEntity entity)
        {
            var persistedEntity = GetById(entity.Id);
            if (entity.LastModified != null)
            {
                persistedEntity.LastModified = entity.LastModified; // this is to trigger concurrency check.
            }

            Remove(persistedEntity);
        }

        /// <summary>
        /// Commit ( Unit of work pattern)
        /// </summary>
        public void Commit()
        {
            PersistenceService.CurrentWorkspace.Commit();
        }

        /// <summary>
        /// Get entity by Id
        /// </summary>
        /// <param name="id">Identify key of entity</param>
        /// <returns>TEntity</returns>
        public virtual TEntity GetById(int id)
        {
            return GetAll().SingleOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Add entity
        /// </summary>
        /// <param name="entity">Entity which will be added to the system</param>
        public virtual void Add(Entity entity)
        {
            var castedEntity = (TEntity)entity;
            Add(castedEntity);
        }

        /// <summary>
        /// Removed entity
        /// </summary>
        /// <param name="entity">Entity which will be removed to the system</param>
        public virtual void Remove(Entity entity)
        {
            var castedEntity = (TEntity)entity;
            Remove(castedEntity);
        }

        /// <summary>
        /// Attached entity
        /// </summary>
        /// <param name="entity">Entity which will be attached to the system</param>
        public virtual void Attach(Entity entity)
        {
            var castedEntity = (TEntity)entity;
            Attach(castedEntity);
        }

        /// <summary>
        /// Get list entity
        /// </summary>
        /// <returns></returns>
        protected virtual IQueryable<TEntity> GetAll()
        {
            return SetSelector(PersistenceService.CurrentWorkspace);
        }

        /// <summary>
        /// Get list entity with other include
        /// </summary>
        /// <param name="includeExpressions">Other include object</param>
        /// <returns></returns>
        protected virtual IQueryable<TEntity> GetAllIncludes(params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            if (includeExpressions.Length > 0)
            {
                return includeExpressions.Aggregate(GetAll(),
                    (current, includeExpression) => current.Include(includeExpression));
            }
            return GetAll();
        }

        /// <summary>
        /// Count entity
        /// </summary>
        /// <param name="where">Condition</param>
        /// <returns>Number of entity which have fitted condition</returns>
        public virtual int Count(Expression<Func<TEntity, bool>> @where = null)
        {
            var set = GetAll();
            return @where != null ? set.Count(@where) : set.Count();
        }

        /// <summary>
        /// Check exists which condition
        /// </summary>
        /// <param name="where">Condition</param>
        /// <returns>True/False</returns>
        public virtual bool CheckExist(Expression<Func<TEntity, bool>> @where = null)
        {
            var set = SetSelector(PersistenceService.CurrentWorkspace);

            return set.CheckExist(@where);
        }

        /// <summary>
        /// Get list entity which condition
        /// </summary>
        /// <param name="predicate">Condition</param>
        /// <returns>List entity</returns>
        public virtual IList<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Where(predicate).ToList();
        }

        /// <summary>
        /// Get list entity which order by and filter
        /// </summary>
        /// <typeparam name="TOrderby"></typeparam>
        /// <param name="filter"></param>
        /// <param name="isDescending"></param>
        /// <param name="order"></param>
        /// <param name="isNoTracking"></param>
        /// <param name="includeExpressions"></param>
        /// <returns></returns>
        public virtual IList<TEntity> Get<TOrderby>(Expression<Func<TEntity, bool>> filter = null, bool isDescending = false, Expression<Func<TEntity, TOrderby>> order = null,
            bool isNoTracking = false, params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            var query = BuildQuery(filter, isDescending, order, isNoTracking, includeExpressions);
            return query.ToList();
        }

        private IQueryable<TEntity> BuildQuery<TOrderby>(Expression<Func<TEntity, bool>> filter = null,
            bool isDescending = false,
            Expression<Func<TEntity, TOrderby>> order = null,
            bool isNoTracking = false,
            params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            IQueryable<TEntity> query;

            if (includeExpressions != null && includeExpressions.Length > 0)
            {
                query = GetAllIncludes(includeExpressions);
            }
            else
            {
                query = GetAll();
            }

            if (isNoTracking)
            {
                query = query.AsNoTracking();
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (order != null)
            {
                query = isDescending ? query.OrderByDescending(order) : query.OrderBy(order);
            }

            return query;
        }
        public virtual IList<TEntity> GetDescending<TOrderby>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TOrderby>> order, bool isNoTracking = false,
            params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            return Get(null, true, order, isNoTracking, includeExpressions);
        }

        public virtual IList<TEntity> GetAscending<TOrderby>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TOrderby>> order, bool isNoTracking = false,
            params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            return Get(null, true, order, isNoTracking, includeExpressions);
        }

        public virtual TEntity Single(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().SingleOrDefault(predicate);
        }

        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate = null)
        {
            if (null == predicate)
            {
                return GetAll().FirstOrDefault();
            }
            return GetAll().Where(predicate).FirstOrDefault();
        }

        public virtual IList<TEntity> ListAll()
        {
            return GetAll().ToList();
        }

        public virtual void InsertOrUpdate(TEntity entity)
        {
            var context = ContextSelector(PersistenceService.CurrentWorkspace);

            context.Entry(entity).State = entity.Id == 0
                ? EntityState.Added
                : EntityState.Modified;
        }

        public virtual List<int> DeleteById(IEnumerable<int> ids)
        {
            var dbContext = ContextSelector(PersistenceService.CurrentWorkspace);
            return dbContext.DeleteByIds<TEntity, int>(ids);
        }

        public virtual int DeleteById(int id)
        {
            var dbContext = ContextSelector(PersistenceService.CurrentWorkspace);
            return dbContext.DeleteById<TEntity, int>(id);
        }

        public virtual void DeleteAll(IEnumerable<TEntity> entities)
        {
            var set = SetSelector(PersistenceService.CurrentWorkspace);
            set.DeleteAll(entities);
        }

        public virtual void DeleteAll(Expression<Func<TEntity, bool>> @where = null)
        {
            var set = SetSelector(PersistenceService.CurrentWorkspace);
            set.DeleteAll(@where);
        }

        public virtual List<LookupItemVo> GetLookup(LookupQuery query, Func<TEntity, LookupItemVo> selector)
        {
            var lookupWhere = BuildLookupCondition(query);

            var lookupList = GetAll().AsNoTracking().Where(lookupWhere);
            var currentRecord = GetAll().AsNoTracking().Where(x => x.Id == query.Id);
            if (!query.IncludeCurrentRecord && currentRecord.SingleOrDefault() != null) // Return single record to reduce the size of return data when first time binding.
            {
                return currentRecord.Select(selector).ToList();
            }

            if (!string.IsNullOrWhiteSpace(query.Query) || !query.IncludeCurrentRecord)
            {
                currentRecord = Enumerable.Empty<TEntity>().AsQueryable();
            }
            var orderBy = "Id";
            if (DisplayColumnForCombobox.Count > 0)
            {
                orderBy = DisplayColumnForCombobox[0];
            }
            var lookupAnonymous = lookupList
                .Union(currentRecord)
                .OrderBy(orderBy)
                .Skip(0)
                .Take(query.Take)
                .Select(selector);
            return lookupAnonymous.ToList();
        }

        public virtual LookupItemVo GetLookupItem(LookupItem lookupItem, Func<TEntity, LookupItemVo> selector)
        {
            var filterWhere = " 1=1";

            if (lookupItem.HierachyItem != null)
            {
                filterWhere += String.Format(" AND {0}.Any(Id = {1})", lookupItem.HierachyItem.Name, lookupItem.HierachyItem.Value);
            }
            var entity = GetAll()
                .AsNoTracking()
                .Where(filterWhere).Select(selector);

            return entity.FirstOrDefault();
        }

        /// <summary>
        /// Build the query for lookup for searching the short name or name, also if there is parent hierarchy lookup filter.
        /// For eg: filter State by CountryID or Region by StateID and CountryID.
        /// For standard lookup the fields are standard and fixed therefore it is ok to use dynamic linq which is not strongly typed.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        protected virtual string BuildLookupCondition(LookupQuery query)
        {
            var where = new StringBuilder();

            //if (!query.IncludeInactiveRecords)
            //{
            //    @where.Append(string.Format(" Active != false AND "));
            //}

            @where.Append("(");
            var innerWhere = new List<string>();
            foreach (var orderByName in DisplayColumnForCombobox)
            {
                var queryDisplayName = String.Format("{0}.Contains(\"{1}\") ", orderByName, query.Query);
                innerWhere.Add(queryDisplayName);
            }
            @where.Append(String.Join(" OR ", innerWhere.ToArray()));
            @where.Append(")");

            if (query.HierachyItems != null)
            {

                foreach (var parentItem in query.HierachyItems.Where(parentItem => parentItem.Value != string.Empty
                                                                                   && parentItem.Value != "-1"
                                                                                   && parentItem.Value != "0"
                                                                                   && !parentItem.IgnoredFilter))
                {
                    var filterValue = parentItem.Value.Replace(",", string.Format(" OR {0} = ", parentItem.Name));
                    @where.Append(string.Format(" AND ( {0} = {1})", parentItem.Name, filterValue));
                }
            }
            return @where.ToString();
        }

        /// <summary>
        /// Get data for create a grid for entity
        /// </summary>
        /// <param name="queryInfo">Query info</param>
        /// 
        /// <returns></returns>
        public virtual MasterfileGridDataVo GetDataForGridMasterfile(IQueryInfo queryInfo)
        {
            BuildSortExpression(queryInfo);
            // Caculate for search string
            var searchString = SearchStringForGetData(queryInfo);
            var finalResult = BuildQueryToGetDataForGrid(queryInfo).AsQueryable().Where(searchString).AsNoTracking();
            queryInfo.TotalRecords = finalResult.Count();

            var data = finalResult.Skip(queryInfo.Skip)
                .Take(queryInfo.Take)
                .ToList();
            return new MasterfileGridDataVo { Data = data, TotalRowCount = queryInfo.TotalRecords };
        }

        /// <summary>
        /// Concrete class have to implement this function to build query for create data in grid
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<ReadOnlyGridVo> BuildQueryToGetDataForGrid(IQueryInfo queryInfo)
        {
            return null;
        }
        /// <summary>
        /// Create search condition to get data in grid
        /// </summary>
        /// <param name="queryInfo"></param>
        /// <returns></returns>
        public virtual string SearchStringForGetData(IQueryInfo queryInfo)
        {
            var searchString = string.Empty;
            if (!string.IsNullOrWhiteSpace(queryInfo.SearchString))
            {

                var searchConditionList = new List<string>();
                queryInfo.SearchString = queryInfo.SearchString.Replace(' ', '+');
                queryInfo.SearchString = Encoding.UTF8.GetString(Convert.FromBase64String(queryInfo.SearchString));
                queryInfo.ParseParameters(queryInfo.SearchString);
                if (!string.IsNullOrWhiteSpace(queryInfo.SearchTerms))
                {
                    var keyword = queryInfo.SearchTerms;
                    var searchCondition = new StringBuilder();

                    searchCondition.Append("(");
                    searchCondition.Append(String.Join(" OR ", SearchColumns.Select(column => string.Format(" {0}.Contains(\"{1}\")", column, keyword)).ToArray()));

                    searchCondition.Append(")");
                    searchConditionList.Add(searchCondition.ToString());
                    searchString = String.Join(" OR ", searchConditionList.ToArray<string>());
                }
            }
            return string.IsNullOrWhiteSpace(searchString) ? " 1 = 1" : searchString;
        }


        /// <summary>
        /// Default sort by ShortOrder then ShortName
        /// </summary>
        /// <param name="queryInfo"></param>
        protected virtual void BuildDefaultSortExpression(IQueryInfo queryInfo)
        {
            if (queryInfo.Sort == null || queryInfo.Sort.Count == 0)
            {
                queryInfo.Sort = new List<Sort> { new Sort { Field = "Id", Dir = "desc" } };
            }
        }

        /// <summary>
        /// This is default sort expression for simple masterfile
        /// Need to custom on web page.
        /// </summary>
        /// <param name="queryInfo"></param>
        protected virtual void BuildSortExpression(IQueryInfo queryInfo)
        {
            BuildDefaultSortExpression(queryInfo);
            queryInfo.Sort.ForEach(x =>
            {
                x.Field = $"{x.Field}";
            });
        }
    }
}