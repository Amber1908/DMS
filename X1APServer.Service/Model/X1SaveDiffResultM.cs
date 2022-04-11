using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Service.DataAnnotaionExt;

namespace X1APServer.Service.Model
{
    public class X1SaveDiffResultM
    {
        public class X1SaveDiffResultReq : REQBase
        {
            /// <summary>
            /// 檢體ID
            /// </summary>
            [Required]
            public int MainID { get; set; }
            /// <summary>
            /// 差異比較結案檔絕對位置
            /// </summary>
            [Required]
            [MaxFileName(50)]
            public string ResultPath { get; set; }
            /// <summary>
            /// 使用者姓名
            /// </summary>
            [Required]
            [MaxLength(30)]
            public string AccName { get; set; }
        }

        public class X1SaveDiffResultRsp : RSPBase
        {

        }
    }
}
