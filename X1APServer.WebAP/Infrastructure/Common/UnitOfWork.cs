using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using X1APServer.Repository.Utility.Interface;

namespace X1APServer.Infrastructure.Common
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbContextProxy _proxy;

        public UnitOfWork(IDbContextProxy proxy)
        {
            _proxy = proxy;
        }

        //[Unity.Dependency]
        //public IDbContextProxy proxy { get; set; }

        /// <summary>
        /// 取回對映的Repository或Service
        /// </summary>  
        /// <typeparam name="T">Repository ex:IMenuRepository</typeparam>
        /// <returns>Repository</returns>
        public T Get<T>() where T : class
        {
#if DEBUG
            bool isInterface = typeof(T).IsInterface;
            if (!isInterface)
            {
                throw new Exception("請使用interface");
            }
#endif
            return DependencyResolver.Current.GetService<T>();
        }

        /// <summary>
        /// 儲存變更並回傳受影響的資料筆數
        /// </summary>
        /// <returns></returns>
        public int Commit()
        {
            return _proxy.SaveChanges();
        }

        /// <summary>
        /// 開始一個Transaction
        /// </summary>
        public void BeginTransaction()
        {
            _proxy.BeginTransaction();
        }

        /// <summary>
        /// RollBack Transaction
        /// </summary>
        public void RollBackTransaction()
        {
            _proxy.RollBackTransaction();
        }

        /// <summary>
        /// Commit Transaction
        /// </summary>
        public void CommitTransaction()
        {
            _proxy.CommitTransaction();
        }

        public void BeginRootTransaction()
        {
            _proxy.BeginRootTransaction();
        }

        public void CommitRootTransaction()
        {
            _proxy.CommitRootTransaction();
        }

        public void RollBackRootTransaction()
        {
            _proxy.RollBackRootTransaction();
        }

        public void BulkCommit()
        {
            _proxy.BulkSaveChanges();
        }
    }
}
