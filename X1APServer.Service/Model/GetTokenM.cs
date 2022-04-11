using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BMDC.Models.Auth.CheckAuthLoginM;
using static BMDC.Models.Auth.GetSysMenuListM;

namespace X1APServer.Service.Model
{
    public class GetTokenM
    {
        public class Request
        {
            /// <summary>
            /// 使用者帳號
            /// </summary>
            [Required]
            public string AccID { get; set; }
            /// <summary>
            /// 網站編號
            /// </summary>
            [Required]
            public int Web_sn { get; set; }
            /// <summary>
            /// 登入時取得的token
            /// </summary>
            [Required]
            public string LoginToken { get; set; }
        }
        
        public class Response : RSPBase
        {
            /// <summary>
            /// idoctor session key
            /// </summary>
            public string SessionKey { get; set; }
        }
    }
}
