using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class X1GetMorphInfoM
    {
        public class X1GetMorphInfoReq : REQBase
        {
            /// <summary>
            /// 檢體 ID
            /// </summary>
            [Required]
            public Nullable<int> ID { get; set; }
            /// <summary>
            /// 檢體類型
            /// </summary>
            [Required]
            public Nullable<X1UserDataSaveM.Type> Type { get; set; }
        }

        public class X1GetMorphInfoRsp : RSPBase
        {
            /// <summary>
            /// 檢體編號
            /// </summary>
            public string SampleNo { get; set; }
            /// <summary>
            /// 病歷號
            /// </summary>
            public string MRNo { get; set; }
            /// <summary>
            /// 圖片路徑
            /// </summary>
            public string[] FolderPath { get; set; }
            /// <summary>
            /// 收件日期
            /// </summary>
            public Nullable<DateTime> RecvDate { get; set; }
            /// <summary>
            /// AI 狀態
            /// </summary>
            public X1ShowListQueryM.State AIState { get; set; }
            /// <summary>
            /// Report 狀態
            /// </summary>
            public X1ShowListQueryM.State ReportState { get; set; }
            /// <summary>
            /// 是否所有人皆標記完成
            /// </summary>
            public bool IsAllFinish { get; set; }
            /// <summary>
            /// 標記中的使用者
            /// </summary>
            public List<MorphUser> MorphUserList { get; set; }
        }

        public class MorphUser
        {
            /// <summary>
            /// 帳號
            /// </summary>
            public string AccID { get; set; }
            /// <summary>
            /// 姓名
            /// </summary>
            public string AccName { get; set; }
            /// <summary>
            /// 是否標記完成 (Y: 完成, N: 標記中)
            /// </summary>
            public string UState { get; set; }
        }
    }
}
