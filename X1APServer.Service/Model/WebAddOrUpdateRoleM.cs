using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BMDC.Models.Auth.AddOrUpdateRole;

namespace X1APServer.Service.Model
{
    public class WebAddOrUpdateRoleM
    {
        public class Request : REQBase
        {
            public AddOrUpdateRoleCotent Data { get; set; }
        }

        public class Response : RSPBase
        {
            public string RoleCode { get; set; }
        }
    }
}
