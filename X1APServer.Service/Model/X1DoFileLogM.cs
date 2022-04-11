using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class X1DoFileLogM
    {
        public class X1DoFileLogReq : REQBase
        {
            /// <summary>
            /// 系統代碼
            /// </summary>
            public string SysCode { get; set; }

            /// <summary>
            /// 紀錄類型
            /// </summary>
            public string LogType { get; set; }

            /// <summary>
            /// 應用系統Key
            /// </summary>
            public string LogKey { get; set; }

            /// <summary>
            /// 建立人員
            /// </summary>
            public string CreateMan { get; set; }

            /// <summary>
            /// 建立者姓名
            /// </summary>
            public string CreateName { get; set; }

            /// <summary>
            /// 異動類型
            /// </summary>
            public string ActionType { get; set; }

            /// <summary>
            /// 異動檔案路徑
            /// </summary>
            public string LogFilePath { get; set; }

            /// <summary>
            /// 異動檔案副檔名
            /// </summary>
            public string LogFileName { get; set; }

            /// <summary>
            /// 異動檔案副檔名
            /// </summary>
            public string LogFileExtName { get; set; }

            /// <summary>
            /// 異動檔案類型
            /// </summary>
            public string LogFileType { get; set; }

        }
        public class X1DoFileLogRsp : RSPBase
        {
        }
    }
}
