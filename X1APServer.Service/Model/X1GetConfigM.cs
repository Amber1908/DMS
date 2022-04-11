using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class X1GetConfigM
    {
        public class X1GetConfigReq : REQBase
        {
            /// <summary>
            /// 檔案代碼
            /// </summary>
            [Required]
            public string FileCode { get; set; }
            /// <summary>
            /// Config儲存相對路徑
            /// </summary>
            [Required]
            [RegularExpression("^[^:]+$", ErrorMessage = "{0}只能為相對路徑")]
            public string FolderPath { get; set; }
            /// <summary>
            /// 要加入權限的 windows user (可讀、可寫、可執行)
            /// </summary>
            public string WindowsUser { get; set; }
        }

        public class X1GetConfigRsp : RSPBase
        {
            /// <summary>
            /// Config儲存絕對路徑
            /// </summary>
            public string FilePath { get; set; }
        }
    }
}
