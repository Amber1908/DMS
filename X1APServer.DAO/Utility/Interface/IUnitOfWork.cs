using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Repository.Utility.Interface
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// 取回對映的Repository或Service
        /// </summary>  
        /// <typeparam name="T">Repository ex:IMenuRepository</typeparam>
        /// <returns>Repository或Service</returns>
        T Get<T>() where T : class;

        /// <summary>
        /// 儲存變更並回傳受影響的資料筆數
        /// </summary>
        /// <returns></returns>
        int Commit();

        /// <summary>
        /// BulkSaveChanges
        /// </summary>
        /// <returns></returns>
        void BulkCommit();

        /// <summary>
        /// 開始一個Transaction
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// RollBack Transaction
        /// </summary>
        void RollBackTransaction();

        /// <summary>
        /// Commit Transaction
        /// </summary>
        void CommitTransaction();

        void BeginRootTransaction();

        void CommitRootTransaction();

        void RollBackRootTransaction();

    }
}
