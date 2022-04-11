using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BMDC.Models.Auth.GetRoleM;

namespace X1APServer.Service.Model
{
    public class WebGetRoleM
    {
        public class Request : REQBase
        {
            /// <summary>
            /// 角色代碼
            /// </summary>
            public string RoleCode { get; set; }
        }

        public class Response : RSPBase
        {
            public Role Data { get; set; }
        }
    }
}
