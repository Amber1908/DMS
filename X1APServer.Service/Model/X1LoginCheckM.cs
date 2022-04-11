using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class X1LoginCheckM
    {
        public class X1LoginCheckReq
        {
            /// <summary>
            /// 登入帳號
            /// </summary>
            [Required(ErrorMessage = "請輸入帳號")]
            public string AccID { get; set; }
            /// <summary>
            /// 登入密碼
            /// </summary>
            [Required(ErrorMessage = "請輸入密碼")]
            public string AccPWD { get; set; }
        }
        public class X1LoginCheckRsp:RSPBase
        {
            /// <summary>
            /// 登入帳號
            /// </summary>
            public string AccID { get; set; }
            /// <summary>
            /// 使用者GUID
            /// </summary>
            public string UserGUID { get; set; }
            /// <summary>
            /// 使用者姓名
            /// </summary>
            public string AccName { get; set; }
            /// <summary>
            /// 使用者職稱
            /// </summary>
            public string AccTitle { get; set; }
            /// <summary>
            /// 強制變更密碼Flag
            /// </summary>
            public bool ChgPwdFlag { get; set; } = false;
            /// <summary>
            /// 角色類型
            /// </summary>
            public List<Role> RoleCode { get; set; }
            /// <summary>
            /// 登入 token
            /// </summary>
            public Guid Token { get; set; }
            /// <summary>
            /// 系統功能清單
            /// </summary>
            public List<MenuListInfo> MenuInfo { get; set; } = new List<MenuListInfo>();
        }

        [Serializable]
        public class MenuListInfo
        {
            /// <summary>
            /// 群組代碼
            /// </summary>
            public string GroupCode { get; set; }
            /// <summary>
            /// 群組名稱
            /// </summary>
            public string GroupName { get; set; }
            /// <summary>
            /// 檢核功能代碼(FN0001: X1標記, FN0002: Report匯出, FN0003: 差異檢核與匯出, FN0004: 共用檔管理)
            /// </summary>
            public string FuncCode { get; set; }
            /// <summary>
            /// 功能名稱
            /// </summary>
            public string FuncName { get; set; }
            /// <summary>
            /// 功能路徑
            /// </summary>
            public string FuncPath { get; set; }
            public string AuthNo1 { get; set; }
            public string AuthNo2 { get; set; }
            public string AuthNo3 { get; set; }
            public string AuthNo4 { get; set; }
            public string AuthNo5 { get; set; }
        }

        [Serializable]
        public class Role
        {
            /// <summary>
            /// 角色代碼
            /// </summary>
            public string RoleCode { get; set; }
            /// <summary>
            /// 角色類型(I:醫檢師, A: Admin)
            /// </summary>
            public string RoleName { get; set; }
        }
    }
}
