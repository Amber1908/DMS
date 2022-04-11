using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class X1GetAllUserMorphListM
    {
        public class X1GetAllUserMorphListReq : REQBase
        {

        }

        public class X1GetAllUserMorphListRsp : RSPBase
        {
            /// <summary>
            /// 抹片清單
            /// </summary>
            public List<MorphData> MorphList { get; set; }
        }

        public class MorphData
        {
            /// <summary>
            /// Main ID
            /// </summary>
            public int MainID { get; set; }
            /// <summary>
            /// 檢體號碼
            /// </summary>
            public string SampleNo { get; set; }
            /// <summary>
            /// 送檢單位
            /// </summary>
            public string MorphFrom { get; set; }
            /// <summary>
            /// 病歷號碼
            /// </summary>
            public string MRNo { get; set; }
            /// <summary>
            /// 收件時間
            /// </summary>
            public DateTime RecvDate { get; set; }
            /// <summary>
            /// AI狀態
            /// </summary>
            public X1ShowListQueryM.State AIState { get; set; }
            /// <summary>
            /// Report狀態
            /// </summary>
            public X1ShowListQueryM.State ReportState { get; set; }
            /// <summary>
            /// 標記人員
            /// </summary>
            public List<User> LabelPersonAccID { get; set; }
            /// <summary>
            /// 圖片絕對路徑
            /// </summary>
            public string[] FolderPath { get; set; }
        }

        public class User
        {
            /// <summary>
            /// 帳號
            /// </summary>
            public string AccID { get; set; }
            /// <summary>
            /// 標記狀態(N: 正在標記, Y: 標記完成)
            /// </summary>
            public string UState { get; set; }
        }
    }
}
