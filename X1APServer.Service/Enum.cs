using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Enum
{
    /// <summary>
    /// AI 狀態
    /// </summary>
    public enum AIState
    {
        /// <summary>
        /// 不用匯出AI
        /// </summary>
        DoNotExportAI = 0,
        /// <summary>
        /// AI 待標記完成
        /// </summary>
        AIWaitingLabelingCompleted = 10,
        /// <summary>
        /// 可以進行差異比較
        /// </summary>
        CanDifferenceComparion = 15,
        /// <summary>
        /// 待差異比較
        /// </summary>
        WaitingForDifferenceComparison = 20,
        /// <summary>
        /// 差異比較中
        /// </summary>
        DifferenceComparing = 25,
        /// <summary>
        /// 已差異比較
        /// </summary>
        DifferenceComparisonCompleted = 30,
        /// <summary>
        /// 已匯出AI
        /// </summary>
        ExportAICompleted = 40
    }

    /// <summary>
    /// Report 狀態
    /// </summary>
    public enum ReportState
    {
        /// <summary>
        /// 不用匯出Report
        /// </summary>
        DoNotExportReport = 0,
        /// <summary>
        /// Report待標記完成
        /// </summary>
        ReportWaitingLabelingCompleted = 10,
        /// <summary>
        /// 待匯出Report
        /// </summary>
        WaitingExportReport = 20,
        /// <summary>
        /// 已匯出Report
        /// </summary>
        ExportReportCompleted = 30,
    }

    /// <summary>
    /// iDoctor 回傳狀態
    /// </summary>
    public enum IDoctorState
    {
        /// <summary>
        /// 成功
        /// </summary>
        SUCESS = 0,
        /// <summary>
        /// 資料庫錯誤
        /// </summary>
        ERROR = 1,
        /// <summary>
        /// 資料庫更新失敗
        /// </summary>
        UPDATEERROR = 2,
        /// <summary>
        /// 用戶資料不存在
        /// </summary>
        FAIL = 1001,
        /// <summary>
        /// 密碼輸入錯誤
        /// </summary>
        PWD_FAIL = 1002,
        /// <summary>
        /// 點數扣除失敗
        /// </summary>
        POINT_FAIL = 1003,
        /// <summary>
        /// 未定義的問題
        /// </summary>
        EXCEPTION = 2001,
        /// <summary>
        /// Email尚未驗證
        /// </summary>
        WARNING = 3001
    }
}
