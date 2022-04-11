using System;
using System.Data.Entity;
using System.Diagnostics;
using X1APServer.Repository.Utility.Interface;

namespace X1APServer.Repository.Utility
{
    public class DbContextProxy : IDbContextProxy
    {
        private DbContext dbContext = null;
        private DbContextTransaction dbTran = null;
        private bool _ignoreInnerCommit = false;

        public DbContextProxy(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void BeginTransaction()
        {
            if (dbTran == null)
            {
                dbTran = dbContext.Database.BeginTransaction();
            }
        }

        public void CommitTransaction()
        {
            if (dbTran != null && !_ignoreInnerCommit)
            {
                dbTran.Commit();
                dbTran.Dispose();
            }
        }

        public void RollBackTransaction()
        {
            if (dbTran != null && !_ignoreInnerCommit)
            {
                dbTran.Rollback();
                dbTran.Dispose();
            }
        }

        public int SaveChanges()
        {
            return dbContext.SaveChanges();
        }

        public void BeginRootTransaction()
        {
            if (dbTran == null)
            {
                dbTran = dbContext.Database.BeginTransaction();
            }

            _ignoreInnerCommit = true;
        }

        public void CommitRootTransaction()
        {
            if (dbTran != null)
            {
                dbTran.Commit();
                dbTran.Dispose();
            }
        }

        public void RollBackRootTransaction()
        {
            if (dbTran != null)
            {
                dbTran.Rollback();
                dbTran.Dispose();
            }
        }

        public void BulkSaveChanges()
        {
            dbContext.BulkSaveChanges();
        }
    }
}
