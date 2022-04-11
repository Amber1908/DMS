using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class X1SaveConfigM
    {
        public class X1SaveConfigReq : REQBase
        {
            /// <summary>
            /// 檔案代碼
            /// </summary>
            [Required]
            [MaxLength(50)]
            public string FileCode { get; set; }
            /// <summary>
            /// 檔案名稱
            /// </summary>
            [Required]
            [MaxLength(50)]
            public string FileName { get; set; }
            /// <summary>
            /// 檔案路徑
            /// </summary>
            [Required]
            public string FilePath { get; set; }
        }

        public class X1SaveConfigRsp : RSPBase
        {

        }
    }
}
