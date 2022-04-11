using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace X1APServer.Repository.Utility.Interface
{
    public interface IBasicRepository<TEntity>
       where TEntity : class
    {
        /// <summary>
        /// 執行sql commmand
        /// *注意：此方法會直接對sql下命令，不會等待unitofwork.commit才執行
        /// </summary>
        /// <param name="sql">sql指令</param>
        /// <param name="parameters">sql參數</param>
        /// <returns>受影響筆數</returns>
        int ExecuteSqlCommand(string sql, params object[] parameters);

        DbRawSqlQuery<T> SqlQuery<T>(string sql, params object[] parameters);

        /// <summary>
        /// Disable creating proxy for DbContext
        /// </summary>
        /// <param name="disable">Disable proxy creation, default is true.</param>
        /// <returns>Return itself for allowing chaining call.</returns>
        IBasicRepository<TEntity> DisableCreateProxy(bool disable = true);

        /// <summary>
        /// 新增一個Entity
        /// </summary>
        /// <param name="entity">Entity 實體</param>
        /// <returns></returns>
        TEntity Create(TEntity entity);

        /// <summary>
        /// 新增一個Entity
        /// </summary>
        /// <param name="entity">Entity 實體</param>
        /// <returns></returns>
        TEntity Create(TEntity entity, string accid);

        /// <summary>
        /// 批次新增Entity
        /// </summary>
        /// <param name="enumerable">Entity 實體集合</param>
        /// <returns></returns>
        IEnumerable<TEntity> Create(IEnumerable<TEntity> enumerable);

        /// <summary>
        /// 使用BulkInsert批次新增Entity
        /// </summary>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        IEnumerable<TEntity> BulkCreate(IEnumerable<TEntity> enumerable, string accid);

        /// <summary>
        /// 取出Entity的Queryable
        /// </summary>
        /// <returns>該ENtity的IQueryable物件</returns>
        IQueryable<TEntity> GetAll();

        IQueryable<TEntity> GetAllTrack();

        /// <summary>
        /// 給予條件後回傳一個Entity
        /// </summary>
        /// <param name="predicate">LINQ條件式</param>
        /// <returns>Entity</returns>
        TEntity Get(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 使用PK取出Entity, 順序視Entity每個Property上掛的[Column(Order = ?)]而定
        /// </summary>
        /// <param name="keyValues">Entity的PK</param>
        /// <returns>Entity</returns>
        TEntity GetByEntityPrimaryKey(params object[] keyValues);

        /// <summary>
        /// 更新Entity
        /// </summary>
        /// <param name="entity">Entity實體</param>
        void Update(TEntity entity);

        /// <summary>
        /// 更新Entity
        /// </summary>
        /// <param name="entity">Entity實體</param>
        void Update(TEntity entity, string accid);

        /// <summary>
        /// 批次更新Entity
        /// </summary>
        /// <param name="entity">Entity實體集合</param>
        /// <returns></returns>
        IEnumerable<TEntity> Update(IEnumerable<TEntity> enumerable);

        /// <summary>
        /// 更新或新增Entity
        /// </summary>
        /// <param name="entity">Entity實體</param>
        void AddOrUpdate(TEntity entity);

        /// <summary>
        /// 更新Entity, 但是不會修改「修改日期」屬性
        /// </summary>
        /// <param name="entity">Entity</param>
        void UpdateWithoutChangeModifyDate(TEntity entity);

        /// <summary>
        /// 使用LINQ表達式來指定要更新的Property
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <param name="conditions">LINQ Expression</param>
        TEntity Update(TEntity entity, params Expression<Func<TEntity, object>>[] conditions);

        /// <summary>
        /// 使用LINQ Expression刪除Entity
        /// </summary>
        /// <param name="condition">LINQ Expression</param>
        void Delete(Expression<Func<TEntity, bool>> condition);

        /// <summary>
        /// 刪除Entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Delete(TEntity entity);

        /// <summary>
        /// 使用PK來刪除Entity, 順序視Entity每個Property上掛的[Column(Order = ?)]而定
        /// </summary>
        /// <param name="keyValues">Entity的PK</param>
        void Delete(params object[] keyValues);

        /// <summary>
        /// 軟刪除Entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void SoftDelete(TEntity entity, string accid);

        /// <summary>
        /// 檢核Entity是否存在
        /// </summary>
        /// <param name="condition">LINQ Expression</param>
        /// <returns>true: 存在, false: 不存在</returns>
        bool Any(Expression<Func<TEntity, bool>> condition);

        IQueryable<TEntity> AllIncluding(params Expression<Func<TEntity, object>>[] includeProperties);
    }
}
