using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class X1AddReportM
    {
        public class X1AddReportReq : REQBase
        {
            /// <summary>
            /// 檢體ID
            /// </summary>
            [Required]
            public int MainID { get; set; }

            /// <summary>
            /// Counting 資料
            /// </summary>
            [Required]
            public List<CountingData> CountingData { get; set; }

            /// <summary>
            /// 特徵圖陣列
            /// </summary>
            [Required]
            public List<CP> CPList { get; set; }
        }

        public class CountingData
        {
            /// <summary>
            /// Counting 名稱
            /// </summary>
            [Required]
            public string Name { get; set; }

            /// <summary>
            /// Counting 值
            /// </summary>
            [Required]
            public decimal Value { get; set; }
        }

        public class CP
        {
            /// <summary>
            /// 特徵圖圖檔絕對位置(工作區)
            /// </summary>
            [Required]
            public string ImagePath { get; set; }
            /// <summary>
            /// 特徵圖描述
            /// </summary>
            [Required]
            [MaxLength(3000)]
            public string Description { get; set; }
            /// <summary>
            /// 原始圖檔相對位置(掃片機)
            /// </summary>
            [Required]
            [RegularExpression("^[^:]+$", ErrorMessage = "{0}只能為相對路徑")]
            public string OriginalImagePath { get; set; }
        }

        public class X1AddReportRsp : RSPBase
        {
        }
    }
}
