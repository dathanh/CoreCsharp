using Framework.DomainModel;
using Framework.DomainModel.ValueObject;
using Framework.Repositories;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Mongo.Repositories
{
    public class ProjectNameMongoRepositoryBase<TEntity> : IMongoDbRepository<TEntity>, IQueryableRepository<TEntity>
       where TEntity : Entity
    {
        protected ProjectNameMongoDbContext ProjectNameMongoDbContext;
        public ProjectNameMongoRepositoryBase(IConfiguration configuration)
        {
            Includes = new Collection<string>();
            SearchColumns = new Collection<string>();
            ProjectNameMongoDbContext = new ProjectNameMongoDbContext(configuration);

        }
        public ProjectNameMongoRepositoryBase(string url, string databaseName)
        {
            ProjectNameMongoDbContext = new ProjectNameMongoDbContext(url, databaseName);
        }


        /// <summary>
        /// List item which is setted for include of entity
        /// </summary>
        public Collection<string> Includes { get; set; }
        /// <summary>
        /// List column in grid which is used for search
        /// </summary>
        public Collection<string> SearchColumns { get; set; }

        public string DisplayColumnForCombobox { get; set; }

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
            ProjectNameMongoDbContext.ProjectNameMongoDatabase.GetCollection<TEntity>(typeof(TEntity).Name).Insert(entity);
        }
        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity which will be updated to the system</param>
        public virtual void Update(TEntity entity)
        {
            ProjectNameMongoDbContext.ProjectNameMongoDatabase.GetCollection<TEntity>(typeof(TEntity).Name).Save(entity);
        }
        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity which will be deleted to the system</param>
        public virtual void Delete(TEntity entity)
        {
            ProjectNameMongoDbContext.ProjectNameMongoDatabase.GetCollection<TEntity>(typeof(TEntity).Name).Remove(MongoDB.Driver.Builders.Query.EQ("Id", entity.Id));
        }
        /// <summary>
        /// Get entity by Id
        /// </summary>
        /// <param name="id">Identify key of entity</param>
        /// <returns>TEntity</returns>
        public virtual TEntity GetById(int id)
        {
            return ProjectNameMongoDbContext.ProjectNameMongoDatabase.GetCollection<TEntity>(typeof(TEntity).Name).FindOneByIdAs<TEntity>(id);
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
        /// Get list entity
        /// </summary>
        /// <returns></returns>
        protected virtual IQueryable<TEntity> GetAll()
        {
            return ProjectNameMongoDbContext.ProjectNameMongoDatabase.GetCollection<TEntity>(typeof(TEntity).Name).AsQueryable<TEntity>();
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
            return GetAll().Any(@where);
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

            query = GetAll();

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
            ProjectNameMongoDbContext.ProjectNameMongoDatabase.GetCollection<TEntity>(typeof(TEntity).Name).Save(entity);
        }

        public virtual List<int> DeleteById(IEnumerable<int> ids)
        {
            throw new NotImplementedException();
        }

        public virtual int DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public virtual void DeleteAll(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public virtual void DeleteAll(Expression<Func<TEntity, bool>> @where = null)
        {
            throw new NotImplementedException();
        }

        public virtual List<LookupItemVo> GetLookup(LookupQuery query, Func<TEntity, LookupItemVo> selector)
        {
            throw new NotImplementedException();
        }

        public virtual LookupItemVo GetLookupItem(LookupItem lookupItem, Func<TEntity, LookupItemVo> selector)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Get data for create a grid for entity
        /// </summary>
        /// <param name="queryInfo">Query info</param>
        /// <returns></returns>
        public virtual MasterfileGridDataVo GetDataForGridMasterfile(IQueryInfo queryInfo)
        {
            throw new NotImplementedException();
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
                x.Field = string.Format("entity.{0}", x.Field);
            });
        }
    }
}
