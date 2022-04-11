using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Repository.Interface;
using X1APServer.Repository.Utility;

namespace X1APServer.Repository
{
    public class SystemFileRepository : X1APBasicRepository<SystemFile>, ISystemFileRepository
    {
        public SystemFileRepository(X1APEntities dbContext) : base(dbContext)
        {
        }

        public SystemFile GetFile(Guid id)
        {
            return Get(f => !f.IsDelete && f.ID == id);
        }
    }
}
