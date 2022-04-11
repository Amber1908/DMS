using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class ImportDataM
    {
        public class ImportDataReq : REQBase
        {
            /// <summary>
            /// 題號行數
            /// </summary>
            public int QuestIDRowNum { get; set; }
            /// <summary>
            /// 資料起始行數
            /// </summary>
            public int DataStartRowNum { get; set; }
            /// <summary>
            /// Excel路徑
            /// </summary>
            public string ExcelPath { get; set; }
            /// <summary>
            /// 忽略錯誤欄位強制匯入
            /// </summary>
            public bool ForceInsert { get; set; } = false;
        }

        public class ImportDataRsp : RSPBase
        {
            /// <summary>
            /// Excel格式錯誤訊息
            /// </summary>
            public List<string> FormatError { get; set; }
            /// <summary>
            /// 是否可以強制匯入
            /// </summary>
            public bool CanForceInsert { get; set; } = false;
        }
    }
}
