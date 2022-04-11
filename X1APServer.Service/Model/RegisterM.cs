using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class WebRegisterM
    {
        public class WebRegisterReq
        {
            /// <summary>
            /// 帳號
            /// </summary>
            [Required]
            [MaxLength(20)]
            public string RequestAccID { get; set; }
            /// <summary>
            /// 密碼
            /// </summary>
            [Required]
            public string AccPWD { get; set; }
            /// <summary>
            /// 姓名
            /// </summary>
            [Required]
            public string AccName { get; set; }
            /// <summary>
            /// 職業
            /// </summary>
            [MaxLength(20)]
            public string AccTitle { get; set; }
            /// <summary>
            /// 電話
            /// </summary>
            //[Phone]
            [MaxLength(20)]
            public string CellPhone { get; set; }
            /// <summary>
            /// Email
            /// </summary>
            //[EmailAddress]
            [MaxLength(50)]
            public string Email { get; set; }
            /// <summary>
            /// 是否為Admin
            /// </summary>
            [Required]
            public bool IsAdmin { get; set; }
            /// <summary>
            /// 職位陣列
            /// </summary>
            [Required]
            public List<string> RoleCode { get; set; }
        }

        public class WebRegisterRsp : RSPBase
        {
            /// <summary>
            /// 帳號
            /// </summary>
            public string AccID { get; set; }
            /// <summary>
            /// 使用者GUID
            /// </summary>
            public string UserGUID { get; set; }
        }
    }
}
