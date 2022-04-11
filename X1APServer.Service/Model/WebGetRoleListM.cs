using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class WebGetRoleListM
    {
        public class WebGetRoleListReq : REQBase
        {

        }

        public class WebGetRoleListRsp : RSPBase
        {
            public List<Role> RoleList { get; set; }
        }

        public class Role
        {
            // 角色代碼
            public string RoleCode { get; set; }
            // 角色名稱
            public string RoleName { get; set; }
            // 角色別名
            public string RoleTitle { get; set; }
        }
    }
}
