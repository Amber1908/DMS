using System;
using System.Data.Entity;
using System.Diagnostics;
using X1APServer.Repository.Utility.Interface;

namespace X1APServer.Repository.Utility
{
    public class DMSShareDbContextProxy : IDMSShareDbContextProxy
    {
        private IDbContextProxy _dbContextProxy;

        public DMSShareDbContextProxy(DMSShareEntities dbContext)
        {
            _dbContextProxy = new DbContextProxy(dbContext);
        }

        public void BeginTransaction()
        {
            _dbContextProxy.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _dbContextProxy.CommitTransaction();
        }

        public void RollBackTransaction()
        {
            _dbContextProxy.RollBackTransaction();
        }

        public int SaveChanges()
        {
            return _dbContextProxy.SaveChanges();
        }

        public void BeginRootTransaction()
        {
            _dbContextProxy.BeginRootTransaction();
        }

        public void CommitRootTransaction()
        {
            _dbContextProxy.CommitRootTransaction();
        }

        public void RollBackRootTransaction()
        {
            _dbContextProxy.RollBackRootTransaction();
        }

        public void BulkSaveChanges()
        {
            _dbContextProxy.BulkSaveChanges();
        }
    }
}
