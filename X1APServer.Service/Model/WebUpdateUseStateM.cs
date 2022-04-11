using BMDC.Models.Auth.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class WebUpdateUseStateM
    {
        public class WebUpdateUseStateReq : REQBase
        {
            /// <summary>
            /// 更新的使用者帳號
            /// </summary>
            [Required]
            public string RequestAccID { get; set; }
            /// <summary>
            /// 更新的狀態
            /// </summary>
            [Required]
            public UserEnum.UseState UseState { get; set; }
        }

        public class WebUpdateUseStateRsp : RSPBase
        {

        }
    }
}
