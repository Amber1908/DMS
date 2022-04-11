using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using X1APServer.Repository.Utility.Interface;
namespace X1APServer.Repository.Utility
{
    public class DMSShareBasicRepository<TEntity> : IBasicRepository<TEntity>
        where TEntity : class
    {
        private BasicRepository<TEntity> _repo;

        public DMSShareBasicRepository(DMSShareEntities dbContext)
        {
            _repo = new BasicRepository<TEntity>(dbContext);
        }

        #region IBasicRepository Member

        public IBasicRepository<TEntity> DisableCreateProxy(bool disable = true)
        {
            return _repo.DisableCreateProxy(disable);
        }

        public int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            return _repo.ExecuteSqlCommand(sql, parameters);
        }

        public DbRawSqlQuery<T> SqlQuery<T>(string sql, params object[] parameters)
        {
            return _repo.SqlQuery<T>(sql, parameters);
        }

        public TEntity Create(TEntity entity)
        {
            return _repo.Create(entity);
        }

        public TEntity Create(TEntity entity, string accid)
        {
            return _repo.Create(entity, accid);
        }

        public IEnumerable<TEntity> Create(IEnumerable<TEntity> enumerable)
        {
            return _repo.Create(enumerable);
        }

        public IQueryable<TEntity> AllIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return _repo.AllIncluding(includeProperties);
        }

        public IQueryable<TEntity> GetAll()
        {
            return _repo.GetAll();
        }

        public TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            return _repo.Get(predicate);
        }

        public TEntity GetByEntityPrimaryKey(params object[] keyValues)
        {
            return _repo.GetByEntityPrimaryKey(keyValues);
        }

        public void Update(TEntity entity)
        {
            _repo.Update(entity);
        }

        public void Update(TEntity entity, string accid)
        {
            _repo.Update(entity, accid);
        }

        public IEnumerable<TEntity> Update(IEnumerable<TEntity> enumerable)
        {
            return _repo.Update(enumerable);
        }

        public void AddOrUpdate(TEntity entity)
        {
            _repo.AddOrUpdate(entity);
        }

        public void UpdateWithoutChangeModifyDate(TEntity entity)
        {
            _repo.UpdateWithoutChangeModifyDate(entity);
        }

        public TEntity Update(TEntity entity, params Expression<Func<TEntity, object>>[] conditions)
        {
            return _repo.Update(entity, conditions);
        }

        public void Delete(Expression<Func<TEntity, bool>> condition)
        {
            _repo.Delete(condition);
        }

        public void Delete(TEntity entity)
        {
            _repo.Delete(entity);
        }

        public void Delete(params object[] keyValues)
        {
            _repo.Delete(keyValues);
        }

        public void SoftDelete(TEntity entity, string accid)
        {
            _repo.SoftDelete(entity, accid);
        }

        public bool Any(Expression<Func<TEntity, bool>> condition)
        {
            return _repo.Any(condition);
        }


        public IEnumerable<TEntity> BulkCreate(IEnumerable<TEntity> enumerable, string accid)
        {
            return _repo.BulkCreate(enumerable, accid);
        }

        public IQueryable<TEntity> GetAllTrack()
        {
            return _repo.GetAllTrack();
        }
        #endregion


    }
}
