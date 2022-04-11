using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Repository.Utility.Interface
{
    public interface IDbContextProxy
    {
        int SaveChanges();

        void BulkSaveChanges();

        void BeginTransaction();

        void CommitTransaction();

        void RollBackTransaction();

        void BeginRootTransaction();

        void CommitRootTransaction();

        void RollBackRootTransaction();
    }
}