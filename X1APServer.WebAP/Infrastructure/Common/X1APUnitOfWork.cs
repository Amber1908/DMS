using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using X1APServer.Repository.Utility.Interface;

namespace X1APServer.Infrastructure.Common
{
    public class X1UnitOfWork : IX1UnitOfWork
    {
        private IUnitOfWork _uow;

        public X1UnitOfWork(IX1DbContextProxy proxy)
        {
            _uow = new UnitOfWork(proxy);
        }

        /// <summary>
        /// 取回對映的Repository或Service
        /// </summary>  
        /// <typeparam name="T">Repository ex:IMenuRepository</typeparam>
        /// <returns>Repository</returns>
        public T Get<T>() where T : class
        {
            return _uow.Get<T>();
        }

        /// <summary>
        /// 儲存變更並回傳受影響的資料筆數
        /// </summary>
        /// <returns></returns>
        public int Commit()
        {
            return _uow.Commit();
        }

        /// <summary>
        /// 開始一個Transaction
        /// </summary>
        public void BeginTransaction()
        {
            _uow.BeginTransaction();
        }

        /// <summary>
        /// RollBack Transaction
        /// </summary>
        public void RollBackTransaction()
        {
            _uow.RollBackTransaction();
        }

        /// <summary>
        /// Commit Transaction
        /// </summary>
        public void CommitTransaction()
        {
            _uow.CommitTransaction();
        }

        public void BeginRootTransaction()
        {
            _uow.BeginRootTransaction();
        }

        public void CommitRootTransaction()
        {
            _uow.CommitRootTransaction();
        }

        public void RollBackRootTransaction()
        {
            _uow.RollBackRootTransaction();
        }

        public void BulkCommit()
        {
            _uow.BulkCommit();
        }
    }
}