using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Repository.Interface;
using X1APServer.Repository.Utility;

namespace X1APServer.Repository
{
    public class DMSSettingRepository : DMSShareBasicRepository<DMSSetting>, IDMSSettingRepository
    {
        public DMSSettingRepository(DMSShareEntities dbContext) : base(dbContext)
        {
        }
    }
}
