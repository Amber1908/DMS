using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class X1GetCountingDataM
    {
        public class X1GetCountingDataReq : REQBase
        {
            /// <summary>
            /// 檢體ID
            /// </summary>
            [Required]
            public int MainID { get; set; }
        }

        public class X1GetCountingDataRsp : RSPBase
        {
            /// <summary>
            /// 檢體ID
            /// </summary>
            public int MainID { get; set; }
            /// <summary>
            /// Counting 資料
            /// </summary>
            public List<X1AddReportM.CountingData> CountingData { get; set; }
            /// <summary>
            /// 新增時間
            /// </summary>
            public System.DateTime CreateDate { get; set; }
            /// <summary>
            /// 新增人員
            /// </summary>
            public string CreateMan { get; set; }
            /// <summary>
            ///  修改時間
            /// </summary>
            public System.DateTime ModifyDate { get; set; }
            /// <summary>
            /// 修改人員
            /// </summary>
            public string ModifyMan { get; set; }
        }
    }
}
