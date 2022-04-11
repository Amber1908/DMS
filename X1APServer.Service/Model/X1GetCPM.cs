using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class X1GetCPM
    {
        public class X1GetCPReq : REQBase
        {
            /// <summary>
            /// 檢體ID
            /// </summary>
            [Required]
            public int MainID { get; set; }
        }

        public class X1GetCPRsp : RSPBase
        {
            /// <summary>
            /// 特徵圖陣列
            /// </summary>
            public List<CP> CPList { get; set; }
        }

        public class CP
        {
            /// <summary>
            /// 特徵圖ID
            /// </summary>
            public int ID { get; set; }
            /// <summary>
            /// 檢體ID
            /// </summary>
            public int MainID { get; set; }
            /// <summary>
            /// 原始路徑(掃片機)
            /// </summary>
            public string OriginalImagePath { get; set; }
            /// <summary>
            /// 檔案名稱
            /// </summary>
            public string FileName { get; set; }
            /// <summary>
            /// 特徵圖描述
            /// </summary>
            public string Description { get; set; }
            /// <summary>
            /// 新增時間
            /// </summary>
            public System.DateTime CreateDate { get; set; }
            /// <summary>
            /// 新增人員
            /// </summary>
            public string CreateMan { get; set; }
        }
    }
}
