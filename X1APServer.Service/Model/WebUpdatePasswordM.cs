using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class WebUpdatePasswordM
    {
        public class WebUpdatePasswordReq : REQBase
        {
            /// <summary>
            /// 舊密碼
            /// </summary>
            [Required]
            public string OldPassword { get; set; }
            /// <summary>
            /// 新密碼
            /// </summary>
            [Required]
            public string NewPassword { get; set; }
        }

        public class WebUpdatePasswordRsp : RSPBase
        {

        }
    }
}
