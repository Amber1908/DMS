using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Service.Enum;

namespace X1APServer.Service.Model
{
    public class X1ShowListQueryM
    {
        public class X1ShowListQueryReq : REQBase
        {
            /// <summary>
            /// 角色類型
            /// I:醫檢師
            /// A:Admin
            /// R:Report
            /// </summary>
            //public string RoleType { get; set; }
            /// <summary>
            /// 要篩選的檢體狀態，空白則取回全部
            /// </summary>
            public List<State> DataState { get; set; }
            /// <summary>
            /// 篩選類型 AI 或 Report
            /// </summary>
            [Required]
            public X1UserDataSaveM.Type Type { get; set; }
        }
        public class X1ShowListQueryRsp : RSPBase
        {
            /// <summary>
            /// 回傳資料列表
            /// </summary>
            public List<X1MainData> DataList { get; set; } = new List<X1MainData>();
        }
        public class X1MainData
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
            /// 使用者狀態
            /// </summary>
            public State UserState { get; set; }
            /// <summary>
            /// AI狀態
            /// </summary>
            public State AIState { get; set; }
            /// <summary>
            /// Report狀態
            /// </summary>
            public State ReportState { get; set; }
            /// <summary>
            /// 差異比較人數
            /// </summary>
            public Nullable<int> PeopleNumber { get; set; }
            /// <summary>
            /// 圖片絕對路徑
            /// </summary>
            public string[] FolderPath { get; set; }
            /// <summary>
            /// 可差異比較?
            /// </summary>
            public bool CanDiff { get; set; }
            /// <summary>
            /// 標記中的使用者清單
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
            /// 標記狀態(Y: 完成, N: 標記中)
            /// </summary>
            public string UState { get; set; }
            /// <summary>
            /// 標記檔類型
            /// </summary>
            public X1UserDataSaveM.Type Type { get; set; }
        }

        public enum State
        {
            /// <summary>
            /// 空的
            /// </summary>
            None = -1,
            /// <summary>
            /// 所有
            /// </summary>
            All = -100,
            /// <summary>
            /// 待標記 L0
            /// </summary>
            WaitingLabeling = PersonalState.WaitingLabeling,
            /// <summary>
            /// 標記中 L1
            /// </summary>
            Labeling = PersonalState.Labeling,
            /// <summary>
            /// 標記完成 L2
            /// </summary>
            LabelingCompleted = PersonalState.LabelingCompleted,
            /// <summary>
            /// 不用匯出Report R0
            /// </summary>
            DoNotExportReport = ResponseReportState.DoNotExportReport,
            /// <summary>
            /// Report待標記完成
            /// </summary>
            ReportWaitingLabelingCompleted = ResponseReportState.ReportWaitingLabelingCompleted,
            /// <summary>
            /// 待匯出Report R2
            /// </summary>
            WaitingExportReport = ResponseReportState.WaitingExportReport,
            /// <summary>
            /// 已匯出Report R3
            /// </summary>
            ExportReportCompleted = ResponseReportState.ExportReportCompleted,
            /// <summary>
            /// 不用匯出AI A0
            /// </summary>
            DoNotExportAI = ResponseAIState.DoNotExportAI,
            /// <summary>
            /// 可以差異比較
            /// </summary>
            CanDifferenceComparion = ResponseAIState.CanDifferenceComparion,
            /// <summary>
            /// AI 待標記完成
            /// </summary>
            AIWaitingLabelingCompleted = ResponseAIState.AIWaitingLabelingCompleted,
            /// <summary>
            /// 待差異比較 A2
            /// </summary>
            WaitingForDifferenceComparison = ResponseAIState.WaitingForDifferenceComparison,
            /// <summary>
            /// 差異比較中
            /// </summary>
            DifferenceComparing = ResponseAIState.DifferenceComparing,
            /// <summary>
            /// 已差異比較 A3
            /// </summary>
            DifferenceComparisonCompleted = ResponseAIState.DifferenceComparisonCompleted,
            /// <summary>
            /// 已匯出AI A4
            /// </summary>
            ExportAICompleted = ResponseAIState.ExportAICompleted
        }

        public enum ResponseAIState
        {
            /// <summary>
            /// 不用匯出AI A0
            /// </summary>
            DoNotExportAI = 200 + AIState.DoNotExportAI,
            /// <summary>
            /// 可以差異比較
            /// </summary>
            CanDifferenceComparion = 200 + AIState.CanDifferenceComparion,
            /// <summary>
            /// AI 待標記完成
            /// </summary>
            AIWaitingLabelingCompleted = 200 + AIState.AIWaitingLabelingCompleted,
            /// <summary>
            /// 待差異比較 A2
            /// </summary>
            WaitingForDifferenceComparison = 200 + AIState.WaitingForDifferenceComparison,
            /// <summary>
            /// 差異比較中
            /// </summary>
            DifferenceComparing = 200 + AIState.DifferenceComparing,
            /// <summary>
            /// 已差異比較 A3
            /// </summary>
            DifferenceComparisonCompleted = 200 + AIState.DifferenceComparisonCompleted,
            /// <summary>
            /// 已匯出AI A4
            /// </summary>
            ExportAICompleted = 200 + AIState.ExportAICompleted
        }

        public enum ResponseReportState
        {
            /// <summary>
            /// 不用匯出Report R0
            /// </summary>
            DoNotExportReport = 100 + ReportState.DoNotExportReport,
            /// <summary>
            /// Report待標記完成
            /// </summary>
            ReportWaitingLabelingCompleted = 100 + ReportState.ReportWaitingLabelingCompleted,
            /// <summary>
            /// 待匯出Report R2
            /// </summary>
            WaitingExportReport = 100 + ReportState.WaitingExportReport,
            /// <summary>
            /// 已匯出Report R3
            /// </summary>
            ExportReportCompleted = 100 + ReportState.ExportReportCompleted,
        }

        public enum PersonalState
        {
            /// <summary>
            /// 待標記 L0
            /// </summary>
            WaitingLabeling = 1,
            /// <summary>
            /// 標記中 L1
            /// </summary>
            Labeling = 2,
            /// <summary>
            /// 標記完成 L2
            /// </summary>
            LabelingCompleted = 3,
        }
    }
}
