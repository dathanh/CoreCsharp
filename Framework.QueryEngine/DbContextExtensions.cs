using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace Framework.QueryEngine
{
    /// <summary>
    ///     The DbContext extensions. Implement some useful method for DbContext class.
    /// </summary>
    public static class DbContextExtensions
    {
        /// <summary>
        /// Use include with multiple object (Eager loading)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public static IQueryable<T> IncludeMultiple<T>(this IQueryable<T> query,
                                                       IEnumerable<Expression<Func<T, object>>> includes)
            where T : class
        {
            if (includes != null)
            {
                query = includes.Aggregate(query,
                                           (current, include) => current.Include(include));
            }

            return query;
        }

        #region Public Methods and Operators

        /// <summary>
        ///     Check whether an entity matched with <paramref name="where" /> exists or not.
        /// </summary>
        /// <param name="dbSet">
        ///     The DbContext.
        /// </param>
        /// <param name="where">
        ///     Where clause.
        /// </param>
        /// <typeparam name="TEntity">
        ///     The entity type.
        /// </typeparam>
        /// <returns>
        ///     <value>
        ///         true
        ///     </value>
        ///     if the entity matched with given condition.
        /// </returns>
        public static bool CheckExist<TEntity>(this DbSet<TEntity> dbSet,
                                               Expression<Func<TEntity, bool>> @where = null) where TEntity : class
        {
            return dbSet.Where(@where).Any();
        }

        /// <summary>
        ///     Count number of entities those matched with <paramref name="where" />.
        /// </summary>
        /// <param name="dbSet">
        ///     The dbSet.
        /// </param>
        /// <param name="where">
        ///     Where expression.
        /// </param>
        /// <typeparam name="TEntity">
        ///     The entity type.
        /// </typeparam>
        /// <returns>
        ///     Number of matched entities.
        /// </returns>
        public static int Count<TEntity>(this DbSet<TEntity> dbSet, Expression<Func<TEntity, bool>> @where = null)
            where TEntity : class
        {
            return dbSet.Where(where).Count();
        }

        /// <summary>
        ///     Delete all entities those matched with <paramref name="where" />.
        /// </summary>
        /// <param name="dbSet">
        ///     The dbSet.
        /// </param>
        /// <param name="where">
        ///     The where clause.
        /// </param>
        /// <typeparam name="TEntity">
        ///     The entity type.
        /// </typeparam>
        /// <returns>
        ///     The affected record.
        /// </returns>
        public static int DeleteAll<TEntity>(this DbSet<TEntity> dbSet, Expression<Func<TEntity, bool>> @where = null)
            where TEntity : class
        {
            var count = 0;
            foreach (var entity in dbSet.Where(@where).AsEnumerable())
            {
                dbSet.Remove(entity);
                count++;
            }

            return count;
        }

        /// <summary>
        ///     Delete entities.
        /// </summary>
        /// <param name="dbSet">
        ///     The dbSet.
        /// </param>
        /// <param name="entities">
        ///     The entities to delete.
        /// </param>
        /// <typeparam name="TEntity">
        ///     The entity type.
        /// </typeparam>
        public static void DeleteAll<TEntity>(this DbSet<TEntity> dbSet, IEnumerable<TEntity> entities)
            where TEntity : class
        {
            foreach (var entity in entities)
            {
                dbSet.Remove(entity);
            }
        }

        /// <summary>
        ///     Delete entity.
        /// </summary>
        /// <param name="dbSet">
        ///     The dbSet.
        /// </param>
        /// <param name="entity"></param>
        /// <typeparam name="TEntity">
        ///     The entity type.
        /// </typeparam>
        public static void Delete<TEntity>(this DbSet<TEntity> dbSet, TEntity entity) where TEntity : class
        {
            dbSet.Remove(entity);
        }

        /// <summary>
        ///     Delete entity by id.
        /// </summary>
        /// <param name="dbContext">
        ///     The dbSet.
        /// </param>
        /// <param name="id">
        ///     The entity id.
        /// </param>
        /// <typeparam name="TEntity">
        ///     The entity type.
        /// </typeparam>
        /// <typeparam name="TId">
        ///     The entity id type.
        /// </typeparam>
        /// <returns>
        ///     Number of affected record (usually 1).
        /// </returns>
        public static int DeleteById<TEntity, TId>(this DbContext dbContext, TId id) where TEntity : class
        {
            var queryString = string.Format("delete {0} where id = @id", dbContext.GetTableName<TEntity>());

            return dbContext.Database.ExecuteSqlCommand(queryString, new SqlParameter("id", id));
        }


        /// <summary>
        ///     Delete entities by their ids.
        /// </summary>
        /// <param name="dbContext">
        ///     The dbSet.
        /// </param>
        /// <param name="ids">
        ///     The entity ids.
        /// </param>
        /// <typeparam name="TEntity">
        ///     The entity type.
        /// </typeparam>
        /// <typeparam name="TId">
        ///     The entity id type.
        /// </typeparam>
        /// <returns>
        ///     List of affect row counts.
        /// </returns>
        public static List<int> DeleteByIds<TEntity, TId>(this DbContext dbContext, IEnumerable<TId> ids)
            where TEntity : class
        {
            return ids.Select(id => DeleteById<TEntity, TId>(dbContext, id)).ToList();
        }


        /// <summary>
        ///     List all entities.
        /// </summary>
        /// <typeparam name="TEntity">
        ///     The entity type.
        /// </typeparam>
        /// <param name="dbSet">
        ///     The dbSet.
        /// </param>
        /// <returns>
        ///     List of entities.
        /// </returns>
        public static List<TEntity> ListAll<TEntity>(this DbQuery<TEntity> dbSet) where TEntity : class
        {
            return dbSet.ToList();
        }

        /// <summary>
        ///     Batch merge list of entities.
        /// </summary>
        /// <param name="dbSet">
        ///     The dbSet.
        /// </param>
        /// <param name="entities">
        ///     The entities.
        /// </param>
        /// <typeparam name="TEntity">
        ///     The entity type.
        /// </typeparam>
        public static void AttachAll<TEntity>(this DbSet<TEntity> dbSet, IEnumerable<TEntity> entities)
            where TEntity : class
        {
            foreach (var entity in entities)
            {
                dbSet.Attach(entity);
            }
        }

        /// <summary>
        ///     Get table name from DvContext
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetTableName<T>(this DbContext context) where T : class
        {
            var mapping = context.Model.FindEntityType(typeof(T)).Relational();
            return mapping.TableName;
        }


        #endregion
    }
}
