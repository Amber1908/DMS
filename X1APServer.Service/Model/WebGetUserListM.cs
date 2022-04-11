using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMDC.Models.Auth;

namespace X1APServer.Service.Model
{
    public class WebGetUserListM
    {
        public class WebGetUserListReq : REQBase
        {
            /// <summary>
            /// 要搜尋的帳號
            /// </summary>
            public string RequestedAccID { get; set; }
            /// <summary>
            /// 要搜尋的姓名
            /// </summary>
            public string AccName { get; set; }
            /// <summary>
            /// 要搜尋的Email
            /// </summary>
            public string Email { get; set; }
            /// <summary>
            /// 要搜尋的電話
            /// </summary>
            public string CellPhone { get; set; }
            /// <summary>
            /// 要搜尋的角色代碼
            /// </summary>
            public List<string> RoleCodes { get; set; }
        }

        public class WebGetUserListRsp : RSPBase
        {
            /// <summary>
            /// 使用者清單
            /// </summary>
            public List<GetUserListM.User> UserList { get; set; }
        }
    }
}