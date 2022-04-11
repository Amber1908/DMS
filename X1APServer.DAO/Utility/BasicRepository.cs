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
    public class BasicRepository<TEntity> : IBasicRepository<TEntity>
        where TEntity : class
    {
        #region Property
        private readonly string[] updateExcludedProperty = new string[] { "CreateDate" };
        private readonly string createDateProperty = "CreateDate";
        private readonly string modifyDateProperty = "ModifyDate";
        private readonly string createManProperty = "CreateMan";
        private readonly string modifyManProperty = "ModifyMan";
        private readonly string deleteDateProperty = "DeleteDate";
        private readonly string deleteManProperty = "DeleteMan";
        private readonly string deleteProperty = "IsDelete";
        protected DbContext dbContext;
        protected DbContextTransaction dbTransaction;
        private readonly Logger logger = LogManager.GetLogger("DBLogger");
        #endregion

        public BasicRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
            // entity logging 
            // 可參考 http://kevintsengtw.blogspot.tw/2013/10/entity-framework-6-logging.html
            //this.dbContext.Database.Log = (log) => Debug.WriteLine(log);
            //this.dbContext.Database.Log = (log) => Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(log);
            //Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(string.Format("{0}{0}-----------------------{0}{0}{0}", Environment.NewLine));
            this.dbContext.Database.Log = (log) => {
                logger.Debug(log);
            };
        }

        #region Private Method
        private DbSet<TEntity> DataBaseSet
        {
            get
            {
                return dbContext.Set<TEntity>();
            }
        }
        #endregion

        #region protected Method
        /// <summary>
        /// Set modify date to system DateTime
        /// </summary>
        /// <param name="entity"></param>
        protected void SetDateModified(TEntity entity)
        {
            PropertyInfo propdatemodified = entity.GetType().GetProperty(modifyDateProperty, BindingFlags.Public | BindingFlags.Instance);

            if (propdatemodified != null)
            {
                propdatemodified.SetValue(entity, DateTime.Now);
            }
        }

        /// <summary>
        /// Set Create Date to system DateTime
        /// </summary>
        /// <param name="entity"></param>
        protected void SetDateCreated(TEntity entity)
        {
            var propdatemodified = entity.GetType().GetProperty(createDateProperty, BindingFlags.Public | BindingFlags.Instance);
            if (propdatemodified != null)
            {
                propdatemodified.SetValue(entity, DateTime.Now);
            }
        }

        /// <summary>
        /// 將updateExcludedProperty中存在的Property設定為不修改
        /// </summary>
        /// <param name="entity"></param>
        protected void SetDefaultDoNotModifyProperty(TEntity entity)
        {
            IEnumerable<string> entityPropertyList = entity.GetType().GetProperties().Select(d => d.Name);

            IEnumerable<string> excludePropertyList = entityPropertyList.Intersect(updateExcludedProperty);

            foreach (string property in excludePropertyList)
            {
                dbContext.Entry(entity).Property(property).IsModified = false;
            }
        }

        /// <summary>
        /// Set property IsModify to false, when property in excludedPropertyNameList
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="excludedPropertyNameList"></param>
        protected void SetDoNotModifyProperty(TEntity entity, IEnumerable<string> excludedPropertyNameList)
        {
            IEnumerable<string> entityPropertyNamelist = entity.GetType().GetProperties().Select(d => d.Name).ToList();

            IEnumerable<string> excludePropertyList = entityPropertyNamelist.Intersect<string>(excludedPropertyNameList);

            foreach (string property in excludePropertyList)
            {
                dbContext.Entry(entity).Property(property).IsModified = false;
            }
        }

        /// <summary>
        /// 設定新增人員
        /// </summary>
        /// <param name="entity"></param>
        protected void SetCreateMan(TEntity entity, string accid)
        {
            var propcreateman = entity.GetType().GetProperty(createManProperty, BindingFlags.Public | BindingFlags.Instance);
            if (propcreateman != null)
            {
                propcreateman.SetValue(entity, accid);
            }
        }

        /// <summary>
        /// 設定更改人員
        /// </summary>
        /// <param name="entity"></param>
        protected void SetModifyMan(TEntity entity, string accid)
        {
            var propModifyMan = entity.GetType().GetProperty(modifyManProperty, BindingFlags.Public | BindingFlags.Instance);
            if (propModifyMan != null)
            {
                propModifyMan.SetValue(entity, accid);
            }
        }

        /// <summary>
        /// 設定刪除
        /// </summary>
        /// <param name="entity"></param>
        protected void SetDelete(TEntity entity)
        {
            var propDelete = entity.GetType().GetProperty(deleteProperty, BindingFlags.Public | BindingFlags.Instance);
            if (propDelete != null)
            {
                propDelete.SetValue(entity, true);
            }
        }

        /// <summary>
        /// 設定刪除日期
        /// </summary>
        /// <param name="entity"></param>
        protected void SetDeleteDate(TEntity entity)
        {
            var propDeleteDate = entity.GetType().GetProperty(deleteDateProperty, BindingFlags.Public | BindingFlags.Instance);
            if (propDeleteDate != null)
            {
                propDeleteDate.SetValue(entity, DateTime.Now);
            }
        }

        /// <summary>
        /// 設定刪除人員
        /// </summary>
        /// <param name="entity"></param>
        protected void SetDeleteMan(TEntity entity, string accid)
        {
            var propDeleteMan = entity.GetType().GetProperty(deleteManProperty, BindingFlags.Public | BindingFlags.Instance);
            if (propDeleteMan != null)
            {
                propDeleteMan.SetValue(entity, accid);
            }
        }
        #endregion

        #region IBasicRepository Member

        public IBasicRepository<TEntity> DisableCreateProxy(bool disable = true)
        {
            this.dbContext.Configuration.ProxyCreationEnabled = !disable;
            return this;
        }

        public int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            return dbContext.Database.ExecuteSqlCommand(sql, parameters);
        }

        public DbRawSqlQuery<T> SqlQuery<T>(string sql, params object[] parameters)
        {
            return dbContext.Database.SqlQuery<T>(sql, parameters);
        }

        public TEntity Create(TEntity entity)
        {
            SetDateCreated(entity);

            SetDateModified(entity);

            DataBaseSet.Add(entity);

            return entity;
        }

        public TEntity Create(TEntity entity, string accid)
        {
            SetCreateMan(entity, accid);

            SetModifyMan(entity, accid);

            return Create(entity);
        }

        public IEnumerable<TEntity> Create(IEnumerable<TEntity> enumerable)
        {
            foreach (TEntity entity in enumerable)
            {
                Create(entity);
            }
            return enumerable;
        }

        public IEnumerable<TEntity> BulkCreate(IEnumerable<TEntity> enumerable, string accid)
        {
            enumerable = enumerable.Select(x =>
            {
                SetDateCreated(x);
                SetDateModified(x);
                SetCreateMan(x, accid);
                SetModifyMan(x, accid);
                return x;
            });
            DataBaseSet.AddRange(enumerable);
            return enumerable;
        }

        public IQueryable<TEntity> AllIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = DataBaseSet;

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query;
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return DataBaseSet.AsNoTracking().AsQueryable();
        }

        public IQueryable<TEntity> GetAllTrack()
        {
            return DataBaseSet;
        }

        public virtual TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            TEntity entity = null;
            if (predicate == null)
            {
                entity = DataBaseSet.AsNoTracking().FirstOrDefault();
            }
            else
            {
                entity = DataBaseSet.AsNoTracking().FirstOrDefault(predicate);
            }
            return entity;
        }

        public virtual TEntity GetByEntityPrimaryKey(params object[] keyValues)
        {
            return DataBaseSet.Find(keyValues);
        }

        public void Update(TEntity entity)
        {
            SetDateModified(entity);

            DataBaseSet.Attach(entity);

            dbContext.Entry(entity).State = EntityState.Modified;

            SetDefaultDoNotModifyProperty(entity);
        }

        public void Update(TEntity entity, string accid)
        {
            SetModifyMan(entity, accid);

            Update(entity);
        }

        public IEnumerable<TEntity> Update(IEnumerable<TEntity> enumerable)
        {
            foreach (TEntity entity in enumerable)
            {
                Update(entity);
            }
            return enumerable;
        }

        public void AddOrUpdate(TEntity entity)
        {
            if (dbContext.Entry(entity).State == EntityState.Detached)
            {
                Create(entity);
            }
            else
            {
                Update(entity);
            }
        }

        public void UpdateWithoutChangeModifyDate(TEntity entity)
        {
            dbContext.Entry(entity).State = EntityState.Modified;

            SetDefaultDoNotModifyProperty(entity);
        }

        public TEntity Update(TEntity entity, params Expression<Func<TEntity, object>>[] conditions)
        {
            dbContext.Set<TEntity>().Attach(entity);
            foreach (var condition in conditions)
            {
                dbContext.Entry(entity).Property(condition).IsModified = true;
            }
            return entity;
        }

        public void Delete(Expression<Func<TEntity, bool>> condition)
        {
            IEnumerable<TEntity> objects = DataBaseSet.Where(condition).AsEnumerable();
            foreach (TEntity entity in objects)
            {
                DataBaseSet.Remove(entity);
            }
        }

        public void Delete(TEntity entity)
        {
            DataBaseSet.Attach(entity);

            DataBaseSet.Remove(entity);
        }

        public void Delete(params object[] keyValues)
        {
            var entity = DataBaseSet.Find(keyValues);
            Delete(entity);
        }

        public void SoftDelete(TEntity entity, string accid)
        {
            SetDeleteDate(entity);

            SetDeleteMan(entity, accid);

            SetDelete(entity);

            DataBaseSet.Attach(entity);

            dbContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual bool Any(Expression<Func<TEntity, bool>> condition)
        {
            return DataBaseSet.Any(condition);
        }


        #endregion


    }
}
