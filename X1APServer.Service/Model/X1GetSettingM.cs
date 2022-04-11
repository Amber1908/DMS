using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class X1GetSettingM
    {
        public class X1GetSettingReq : REQBase
        {
            /// <summary>
            /// 設定名稱
            /// </summary>
            [Required]
            public string Name { get; set; }
        }

        public class X1GetSettingRsp :RSPBase
        {
            /// <summary>
            /// 設定名稱
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// 設定值
            /// </summary>
            public string Value { get; set; }
        }
    }
}
