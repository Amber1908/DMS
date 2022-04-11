using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class X1UserDataGetM
    {
        public class X1UserDataGetReq
        {
            /// <summary>
            /// 查詢帳戶 Email (不帶值時查全部)
            /// </summary>
            public string AccountID { get; set; }
        }

        public class X1UserDataGetRsp : RSPBase
        {
            /// <summary>
            /// 用戶資訊
            /// </summary>
            public List<X1User> UserList { get; set; }
        }

        public class X1User
        {
            /// <summary>
            /// 用戶代碼
            /// </summary>
            public int UserID { get; set; }
            /// <summary>
            /// 用戶帳號 (Email)
            /// </summary>
            public string AccID { get; set; }
            /// <summary>
            /// 用戶姓名
            /// </summary>
            public string AccName { get; set; }
            /// <summary>
            /// 用戶身分別
            /// </summary>
            public string Role { get; set; }
            /// <summary>
            /// 醫師(醫檢師)代碼
            /// </summary>
            public string DoctorNo { get; set; }
        }
        
    }
}
