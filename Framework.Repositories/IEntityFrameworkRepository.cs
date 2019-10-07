using Framework.DomainModel;
using Framework.DomainModel.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Framework.Repositories
{
    public interface IEntityFrameworkRepository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        void Remove(TEntity entity);
        void Attach(TEntity entity);
        void Commit();
    }

    public interface IQueryableRepository<TEntity> where TEntity : Entity
    {

        /// <summary>
        ///     The count number of entity.
        /// </summary>
        /// <param name="where">searching condition</param>
        /// <returns>number of record </returns>
        int Count(Expression<Func<TEntity, bool>> @where = null);


        /// <summary>
        ///     The check entity exist.
        /// </summary>
        /// <param name="where">Condition to check</param>
        /// <returns>true if exist, false if not</returns>
        bool CheckExist(Expression<Func<TEntity, bool>> @where = null);


        /// <summary>
        ///     Get entity by expression
        /// </summary>
        /// <param name="predicate">condition to get</param>
        /// <returns>List of  entities that qualify condition</returns>
        IList<TEntity> Get(Expression<Func<TEntity, bool>> predicate);




        /// <summary>
        ///     Get paged Descending entities
        /// </summary>
        /// <typeparam name="TOrderby"></typeparam>
        /// <param name="filter">Filtering conditions</param>
        /// <param name="order">>Ordering expression</param>
        /// <param name="isNoTracking">Should track returns enities in dbContext</param>
        /// <param name="includeExpressions">Eager load options</param>
        /// <returns>List of entities</returns>
        IList<TEntity> GetDescending<TOrderby>(Expression<Func<TEntity, bool>> filter,
                                               Expression<Func<TEntity, TOrderby>> order,
                                               bool isNoTracking = false,
                                               params Expression<Func<TEntity, object>>[] includeExpressions);


        /// <summary>
        ///     Get paged Ascending entities
        /// </summary>
        /// <typeparam name="TOrderby">Order field of entity</typeparam>
        /// <param name="filter">Filtering condition</param>
        /// <param name="order">Order expression</param>
        /// <param name="isNoTracking">Should track returns enities in dbContext</param>
        /// <param name="includeExpressions">Eager load options</param>
        /// <returns>List of entities</returns>
        IList<TEntity> GetAscending<TOrderby>(Expression<Func<TEntity, bool>> filter,
                                              Expression<Func<TEntity, TOrderby>> order,
                                              bool isNoTracking = false,
                                              params Expression<Func<TEntity, object>>[] includeExpressions);




        /// <summary>
        ///     Get entity by expression
        /// </summary>
        /// <param name="predicate">Condition expression to search</param>
        /// <returns>First entity</returns>
        TEntity Single(Expression<Func<TEntity, bool>> predicate);


        /// <summary>
        ///     Get entity by expression
        /// </summary>
        /// <param name="predicate">Condition expression to search</param>
        /// <returns>First entity or default</returns>
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate = null);


        /// <summary>
        ///     List all entities in database
        /// </summary>
        /// <returns>List of entities</returns>
        IList<TEntity> ListAll();

        /// <summary>
        ///     Insert or Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void InsertOrUpdate(TEntity entity);


        /// <summary>
        ///     Delete a list of entities
        /// </summary>
        /// <param name="ids">List of entities</param>
        /// <returns>Affected record count.</returns>
        List<int> DeleteById(IEnumerable<int> ids);


        /// <summary>
        ///     Delete an entity by its id.
        /// </summary>
        /// <param name="id">Id to delete</param>
        /// <returns>Affected record count.</returns>
        int DeleteById(int id);


        /// <summary>
        ///     Delete a list of entity
        /// </summary>
        /// <param name="entities">List of entities to delete</param>
        void DeleteAll(IEnumerable<TEntity> entities);


        /// <summary>
        ///     Delete entity base on condition
        /// </summary>
        /// <param name="where">Condition to delete</param>
        void DeleteAll(Expression<Func<TEntity, bool>> @where = null);

        /// <summary>
        ///     Get item for lookup
        /// </summary>
        /// <returns></returns>
        List<LookupItemVo> GetLookup(LookupQuery query, Func<TEntity, LookupItemVo> selector);


        /// <summary>
        /// Get parent lookup item from child item
        /// </summary>
        /// <param name="lookupItem"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        LookupItemVo GetLookupItem(LookupItem lookupItem, Func<TEntity, LookupItemVo> selector);

        /// <summary>
        /// Get data for grid
        /// </summary>
        /// <param name="queryInfo"></param>
        /// <returns></returns>
        MasterfileGridDataVo GetDataForGridMasterfile(IQueryInfo queryInfo);
    }
}
