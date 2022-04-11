using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class WebUpdateUserM
    {
        public class WebUpdateUserReq : REQBase
        {
            /// <summary>
            /// 要更新資料的帳號
            /// </summary>
            [Required]
            public string RequestAccID { get; set; }
            /// <summary>
            /// 姓名
            /// </summary>
            [MaxLength(30)]
            public string AccName { get; set; }
            /// <summary>
            /// 職業
            /// </summary>
            [MaxLength(20)]
            public string AccTitle { get; set; }
            [MaxLength(50)]
            public string Email { get; set; }
            /// <summary>
            /// 電話
            /// </summary>
            [MaxLength(20)]
            public string CellPhone { get; set; }
            /// <summary>
            /// 角色類別代碼
            /// </summary>
            [Required]
            public List<string> RoleCode { get; set; }
            /// <summary>
            /// 醫生/醫檢師 代碼
            /// </summary>
            public string DoctorNo { get; set; }
            /// <summary>
            /// 醫檢師資歷
            /// </summary>
            public int Senior { get; set; }

        }
        public class WebUpdateUserRsp : RSPBase
        {
        }
    }
}
