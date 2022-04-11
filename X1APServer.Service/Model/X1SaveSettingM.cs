using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class X1SaveSettingM
    {
        public class X1SaveSettingReq : REQBase
        {
            /// <summary>
            /// 設定名稱
            /// </summary>
            [Required]
            [MaxLength(50)]
            public string Name { get; set; }
            /// <summary>
            /// 設定值
            /// </summary>
            [Required]
            [MaxLength(50)]
            public string Value { get; set; }
        }

        public class X1SaveSettingRsp : RSPBase
        {
            /// <summary>
            /// ID
            /// </summary>
            public int ID { get; set; }
        }
    }
}
