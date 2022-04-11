using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class X1GetDiffResultM
    {
        public class X1GetDiffResultReq : REQBase
        {
            /// <summary>
            /// 檢體ID
            /// </summary>
            [Required]
            public int MainID { get; set; }
            /// <summary>
            /// 儲存結案檔的相對路徑(工作區)
            /// </summary>
            [Required]
            [RegularExpression("^[^:]+$", ErrorMessage = "{0}不得為絕對路徑")]
            public string ResultPath { get; set; }
        }

        public class X1GetDiffResultRsp : RSPBase
        {
            /// <summary>
            /// 儲存結案檔的絕對路徑(工作區)
            /// </summary>
            public string ResultPath { get; set; }
        }
    }
}
