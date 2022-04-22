using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Service.Model;

namespace X1APServer.Service.Interface
{
    public interface IReportService
    {
        /// <summary>
        /// 取得單一Report
        /// </summary>
        /// <param name="ReqData"></param>
        /// <param name="x1GetReportsRsp"></param>
        /// <param name="serverImageFolder"></param>
        /// <returns></returns>
        RSPBase GetReport(GetReportM.GetReportReq ReqData, ref GetReportM.GetReportRsp x1GetReportsRsp, string serverImageFolder);
        /// <summary>
        /// 取得單個 Report 歷史清單
        /// </summary>
        /// <param name="ReqData"></param>
        /// <param name="x1GetReportsRsp"></param>
        /// <returns></returns>
        RSPBase GetReports(GetReportsM.GetReportsReq ReqData, ref GetReportsM.GetReportsRsp x1GetReportsRsp);
        /// <summary>
        /// 更新 Report 答案
        /// </summary>
        /// <returns></returns>
        RSPBase UpdateReport(UpdateReportM.UpdateReportReq ReqData, ref UpdateReportM.UpdateReportRsp x1SaveReportRsp);
        /// <summary>
        /// 更新檢驗單狀態
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        RSPBase UpdateReportStatus(UpdateReportStatusM.UpdateReportStatusReq request);
        /// <summary>
        /// 新增 Report
        /// </summary>
        /// <returns></returns>
        RSPBase AddGeneralReport(AddGeneralReportM.AddGeneralReportReq ReqData, ref AddGeneralReportM.AddGeneralReportRsp x1AddGeneralReportRsp);
        /// <summary>
        /// 匯出 Report
        /// </summary>
        /// <param name="ReqData"></param>
        /// <param name="exportReportRsp"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        RSPBase ExportReport(ExportReportM.ExportReportReq ReqData, ref ExportReportM.ExportReportRsp exportReportRsp, string filePath);
        /// <summary>
        /// 取得匯出Report清單
        /// </summary>
        /// <param name="ReqData"></param>
        /// <param name="getExportReportList"></param>
        /// <returns></returns>
        RSPBase GetExportReportListLazy(GetExportReportListLazyM.GetExportReportListLazyReq ReqData, ref GetExportReportListLazyM.GetExportReportListLazyRsp getExportReportList);
        /// <summary>
        /// 取得問題清單
        /// </summary>
        /// <param name="ReqData"></param>
        /// <param name="getQuestionListRsp"></param>
        /// <returns></returns>
        RSPBase GetQuestionList(GetQuestionListM.GetQuestionListReq ReqData, ref GetQuestionListM.GetQuestionListRsp getQuestionListRsp);
        /// <summary>
        /// 更新 Report File
        /// </summary>
        /// <param name="ReqData"></param>
        /// <param name="updateReportFileRsp"></param>
        /// <returns></returns>
        RSPBase UpdateReportFile(UpdateReportFileM.UpdateReportFileReq ReqData, ref UpdateReportFileM.UpdateReportFileRsp updateReportFileRsp);
        /// <summary>
        /// 取得 Convetion 最近的匯出時間
        /// </summary>
        /// <param name="request"></param>
        /// <param name="getLatestCBCExportDateRsp"></param>
        /// <returns></returns>
        RSPBase GetLatestCBCExportDate(GetLatestCBCExportDateM.GetLatestCBCExportDateReq request, ref GetLatestCBCExportDateM.GetLatestCBCExportDateRsp getLatestCBCExportDateRsp);
        /// <summary>
        /// 解鎖 Report
        /// </summary>
        /// <param name="request"></param>
        /// <param name="unlockReportRsp"></param>
        /// <returns></returns>
        RSPBase UnlockReport(UnlockReportM.UnlockReportReq request, ref UnlockReportM.UnlockReportRsp unlockReportRsp);
        /// <summary>
        /// 結案 Report
        /// </summary>
        /// <param name="request"></param>
        /// <param name="closeoutReportRsp"></param>
        /// <returns></returns>
        RSPBase CloseoutReport(CloseoutReportM.CloseoutReportReq request, ref CloseoutReportM.CloseoutReportRsp closeoutReportRsp);
        /// <summary>
        /// 取得 Report Main 清單
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        RSPBase GetAllReportMain(GetAllReportMainM.GetAllReportMainReq request, ref GetAllReportMainM.GetAllReportMainRsp response);
        /// <summary>
        /// 取得 Report Main
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        RSPBase GetReportMain(GetReportMainM.GetReportMainReq request, ref GetReportMainM.GetReportMainRsp response);
        /// <summary>
        /// 更新 Report Main
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        RSPBase UpdateReportMain(UpdateReportMainM.UpdateReportMainReq request, ref UpdateReportMainM.UpdateReportMainRsp response);
        /// <summary>
        /// 新增 Report Main
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        RSPBase AddReportMain(AddReportMainM.AddReportMainReq request, ref AddReportMainM.AddReportMainRsp response, string dbname);
        /// <summary>
        /// 刪除 Report Main
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        RSPBase DeleteReportMain(DeleteReportMainM.DeleteReportMainReq request, ref DeleteReportMainM.DeleteReportMainRsp response);
        /// <summary>
        /// 發布 Report
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        RSPBase PublishReport(PublishReportM.PublishReportReq request, ref PublishReportM.PublishReportRsp response);
        /// <summary>
        /// 取得所有版本的 Report
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        RSPBase GetAllVersionReport(GetAllVerionReportM.GetAllVersionReportReq request, ref GetAllVerionReportM.GetAllVersionReportRsp response);
        /// <summary>
        /// 加入題目檔案
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        RSPBase AddReportMainFile(AddReportMainFileM.AddReportMainFileReq request, ref AddReportMainFileM.AddReportMainFileRsp response);
        /// <summary>
        /// 取得 Report Main 檔案
        /// </summary>
        /// <param name="RQID"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        GetReportMainFileM.GetReportMainFileRsp GetReportMainFile(int RQID, string fileName);
        /// <summary>
        /// 新增 Report 答案檔案
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        RSPBase AddReportAnsFile(AddReportAnsFileM.AddReportAnsFileReq request, ref AddReportAnsFileM.AddReportAnsFileRsp response);
        /// <summary>
        /// 取得 Report 答案檔案
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        GetReportAnsFileM.GetReportAnsFileRsp GetReportAnsFile(int ID);
        /// <summary>
        /// 匯出CodingBook
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <param name="savePath">儲存位置</param>
        /// <returns></returns>
        RSPBase ExportCodingBook(ExportCodingBookM.ExportCodingBookReq request, ref ExportCodingBookM.ExportCodingBookRsp response, string savePath);
        /// <summary>
        /// 匯出資料
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <param name="savePath"></param>
        /// <returns></returns>
        RSPBase ExportData(ExportDataM.ExportDataReq request, ref ExportDataM.ExportDataRsp response, string savePath);
        /// <summary>
        /// 加入Report權限
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        RSPBase AddReportAuth(AddReportAuthM.AddReportAuthReq request, ref AddReportAuthM.AddReportAuthRsp response);
        /// <summary>
        /// 匯入Excel資料
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        RSPBase ImportData(ImportDataM.ImportDataReq request, ref ImportDataM.ImportDataRsp response);
        /// <summary>
        /// 取得報告模板名稱清單
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        RSPBase GetReportTemplateList(GetReportTemplateListM.GetReportTemplateListReq request, ref GetReportTemplateListM.GetReportTemplateListRsp response);
        /// <summary>
        /// 取得報告模板資料
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        RSPBase GetReportTemplate(GetReportTemplateM.Request request, ref GetReportTemplateM.Response response, string baseUrl);
        /// <summary>
        /// 取得匯出模板額外問題清單
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        RSPBase GetETemplateEQuestList(GetETemplateEQuestListM.Request request, ref GetETemplateEQuestListM.Response response);
        /// <summary>
        /// 匯出子宮頸國健署資料
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <param name="rootPath"></param>
        /// <returns></returns>
        RSPBase ExportCervixData(ExportCervixDataM.Request request, ref ExportCervixDataM.Response response, string rootPath,string WebDB);
        /// <summary>
        /// 匯出Excel
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        RSPBase ExportExcel(ExportExcelM.Request request, ref ExportExcelM.Response response, string rootPath);
        /// <summary>
        /// 取得問題資料
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        RSPBase CheckQuestNo(CheckQuestNo.Request request, ref CheckQuestNo.Response response);
        /// <summary>
        /// 取得關注問題
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        RSPBase GetPinQuestData(GetPinQuestM.Request request, ref GetPinQuestM.Response response);
        /// <summary>
        /// 更新關注問題
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        RSPBase UpdatePinnedQuest(UpdatePinnedQuestM.Request request, ref UpdatePinnedQuestM.Response response);
        /// <summary>
        /// 取得所有關注問題
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        RSPBase GetPinnedQuestList(GetPinnedQuestListM.Request request, ref GetPinnedQuestListM.Response response);
        /// <summary>
        /// 更新病患個人關注問題
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        RSPBase UpdatePersonalPinnedQuest(UpdatePersonalPinnedQuestM.Request request, ref UpdatePersonalPinnedQuestM.Response response);
        /// <summary>
        /// 取得病患個人關注問題
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        RSPBase GetPersonalPinnedQuestList(GetPersonalPinnedQuestListM.Request request, ref GetPersonalPinnedQuestListM.Response response);
        /// <summary>
        /// 刪除所有個人關注問題
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        RSPBase DeletePersonalPinnedQuest(DeletePersonalPinnedQuestM.Request request, ref DeletePersonalPinnedQuestM.Response response);
        
    
    }
}
