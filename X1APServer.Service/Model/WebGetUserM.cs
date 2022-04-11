using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static X1APServer.Service.Model.X1LoginCheckM;

namespace X1APServer.Service.Model
{
    public class WebGetUserM
    {
        public class WebGetUserReq : REQBase
        {
            /// <summary>
            /// 查詢的使用者帳號
            /// </summary>
            [Required]
            public string RequestAccID { get; set; }
        }

        public class WebGetUserRsp : RSPBase
        {
            /// <summary>
            /// 帳號
            /// </summary>
            public string AccID { get; set; }
            /// <summary>
            /// 使用者GUID
            /// </summary>
            public string UserGUID { get; set; }
            /// <summary>
            /// 姓名
            /// </summary>
            public string AccName { get; set; }
            /// <summary>
            /// 職業
            /// </summary>
            public string AccTitle { get; set; }
            public string Email { get; set; }
            /// <summary>
            /// 電話
            /// </summary>
            public string CellPhone { get; set; }
            /// <summary>
            /// 使用狀態
            /// </summary>
            public string UseState { get; set; }
            /// <summary>
            /// 使用者狀態
            /// </summary>
            public string UserState { get; set; }
            /// <summary>
            /// 是否為Admin(Y: 是, N: 否)
            /// </summary>
            public string IsAdmin { get; set; }
            /// <summary>
            /// 使用狀態更新時間
            /// </summary>
            public DateTime StateUDate { get; set; }
            /// <summary>
            /// 使用者狀態更新時間
            /// </summary>
            public DateTime StatusUDate { get; set; }
            
            /// <summary>
            /// 新增時間
            /// </summary>
            public DateTime CreateDate { get; set; }
            /// <summary>
            /// 更新時間
            /// </summary>
            public DateTime ModifyDate { get; set; }
            /// <summary>
            /// 醫生/醫檢師 代碼
            /// </summary>
            public string DoctorNo { get; set; }
            /// <summary>
            /// 醫檢師資歷
            /// </summary>
            public int Senior { get; set; }
            /// <summary>
            /// 使用者角色清單
            /// </summary>
            public List<X1APServer.Service.Model.WebGetRoleListM.Role> RoleList { get; set; }
            /// <summary>
            /// 系統功能清單
            /// </summary>
            public List<MenuListInfo> MenuInfo { get; set; } = new List<MenuListInfo>();
        }
    }
}
