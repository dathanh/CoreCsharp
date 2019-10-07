using Database.Persistance.Tenants;
using Framework.BusinessRule;
using Framework.DomainModel;
using Framework.DomainModel.ValueObject;
using Framework.Exceptions;
using Framework.Repositories;
using ServiceLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ServiceLayer
{
    public class MasterFileService<TEntity> : IMasterFileService<TEntity>
        where TEntity : Entity
    {
        public IRepository<TEntity> Repository { get; protected set; }
        public IQueryableRepository<TEntity> QueryableRepository { get; protected set; }
        public IBusinessRuleSet<TEntity> BusinessRuleSet { get; protected set; }

        public MasterFileService(IRepository<TEntity> service,
            IQueryableRepository<TEntity> queryableService,
            ITenantPersistenceService tenantPersistentService,
            IBusinessRuleSet<TEntity> businessRuleSet = null)
        {
            Repository = service;
            QueryableRepository = queryableService;
            BusinessRuleSet = businessRuleSet;
            TenantPersistenceService = tenantPersistentService;
        }

        public ITenantPersistenceService TenantPersistenceService { get; protected set; }

        public virtual TEntity GetById(int id)
        {
            var entity = Repository.GetById(id);
            return entity;
        }

        protected virtual void ValidateBusinessRules(TEntity entity)
        {
            if (BusinessRuleSet != null)
            {
                var businessRules = BusinessRuleSet.ExecuteRules(entity, null);
                if (businessRules.Any(x => x.IsFailed))
                {
                    // Give messages on every rule that failed
                    throw new BusinessRuleException("BussinessGenericErrorMessageKey", businessRules.Where(x => x.IsFailed).ToArray());
                }
            }
        }

        public virtual TEntity Add(TEntity entity)
        {
            ValidateBusinessRules(entity);
            Repository.Add(entity);
            if (Repository is IEntityFrameworkRepository<TEntity>)
            {
                (Repository as IEntityFrameworkRepository<TEntity>).Commit();
            }

            return entity;
        }

        public virtual TEntity Update(TEntity entity)
        {
            ValidateBusinessRules(entity);

            Repository.Update(entity);
            if (Repository is IEntityFrameworkRepository<TEntity>)
            {
                (Repository as IEntityFrameworkRepository<TEntity>).Commit();
            }
            return entity;
        }

        public virtual TEntity Single(Expression<Func<TEntity, bool>> predicate)
        {
            var entity = QueryableRepository.Single(predicate);
            return entity;
        }

        public virtual void Delete(TEntity model)
        {
            Repository.Delete(model);
            if (Repository is IEntityFrameworkRepository<TEntity>)
            {
                (Repository as IEntityFrameworkRepository<TEntity>).Commit();
            }
        }

        public virtual void DeleteById(int id)
        {
            var entity = GetById(id);
            if (null != entity)
            {
                Delete(entity);
            }
        }

        public virtual IList<TEntity> ListAll()
        {
            var allEntities = QueryableRepository.ListAll();
            return allEntities;
        }

        public virtual int Count(Expression<Func<TEntity, bool>> @where = null)
        {
            var result = QueryableRepository.Count(@where);
            return result;
        }

        public virtual bool CheckExist(Expression<Func<TEntity, bool>> @where = null)
        {
            var result = QueryableRepository.CheckExist(@where);
            return result;
        }

        public virtual IList<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
        {
            var result = QueryableRepository.Get(predicate);
            return result;
        }



        public virtual IList<TEntity> GetDescending<TOrderby>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TOrderby>> order, bool isNoTracking = false,
            params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            var result = QueryableRepository.GetDescending(filter, order, isNoTracking, includeExpressions);
            return result;
        }

        public virtual IList<TEntity> GetAscending<TOrderby>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TOrderby>> order, bool isNoTracking = false,
            params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            var result = QueryableRepository.GetAscending(filter, order, isNoTracking, includeExpressions);
            return result;
        }

        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate = null)
        {
            var result = QueryableRepository.FirstOrDefault(predicate);
            return result;
        }


        public virtual void InsertOrUpdate(TEntity entity)
        {
            ValidateBusinessRules(entity);

            QueryableRepository.InsertOrUpdate(entity);
            if (Repository is IEntityFrameworkRepository<TEntity>)
            {
                (Repository as IEntityFrameworkRepository<TEntity>).Commit();
            }
        }

        public virtual List<int> DeleteById(IEnumerable<int> ids)
        {
            return QueryableRepository.DeleteById(ids);
        }

        public virtual void DeleteAll(IEnumerable<TEntity> entities)
        {
            QueryableRepository.DeleteAll(entities);
            if (Repository is IEntityFrameworkRepository<TEntity>)
            {
                (Repository as IEntityFrameworkRepository<TEntity>).Commit();
            }
        }

        public virtual void DeleteAll(Expression<Func<TEntity, bool>> @where = null)
        {
            QueryableRepository.DeleteAll(@where);
            if (Repository is IEntityFrameworkRepository<TEntity>)
            {
                (Repository as IEntityFrameworkRepository<TEntity>).Commit();
            }
        }

        public virtual List<LookupItemVo> GetLookup(LookupQuery query, Func<TEntity, LookupItemVo> selector)
        {
            var result = QueryableRepository.GetLookup(query, selector);
            return result;
        }

        public virtual LookupItemVo GetLookupItem(LookupItem lookupItem, Func<TEntity, LookupItemVo> selector)
        {
            return QueryableRepository.GetLookupItem(lookupItem, selector);
        }

        public virtual MasterfileGridDataVo GetDataForGridMasterFile(IQueryInfo queryInfo)
        {
            var result = QueryableRepository.GetDataForGridMasterfile(queryInfo);
            return result;
        }

    }
}