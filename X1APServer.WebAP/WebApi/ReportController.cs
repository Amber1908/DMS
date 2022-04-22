using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Windows.Forms;
using NLog;
using WebApplication1.Infrastructure.Common;
using WebApplication1.Infrastructure.Common.Interface;
using WebApplication1.Infrastructure.Filters;
using WebApplication1.Infrastructure.Utility;
using WebApplication1.Misc;
using X1APServer.Repository.Utility.Interface;
using X1APServer.Service;
using X1APServer.Service.Interface;
using X1APServer.Service.Model;

namespace WebApplication1.WebApi
{
    public class ReportController : ApiController
    {
        private readonly IDMSShareUnitOfWork _uow;
        private readonly IIDoctorService _idoctorSvc;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private IReportService _svc = null;
        private IFrameRequest _frameReq;
        private AuthHandler _authHandler = null;
        private Guid guid;

        public ReportController(IReportService svc, IFrameRequest frameReq, IDMSShareUnitOfWork uow, IIDoctorService idoctorSvc)
        {
            _idoctorSvc = idoctorSvc;
            _uow = uow;
            _svc = svc;
            _frameReq = frameReq;
            _authHandler = new AuthHandler(AuthHandler.System.X1Web);
            guid = Guid.NewGuid();
        }

        /// <summary>
        /// 取得單一Report
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<GetReportM.GetReportRsp> GetReport(GetReportM.GetReportReq request)
        {

            GetReportM.GetReportRsp retResp = new GetReportM.GetReportRsp();

            if (ModelState.IsValid)
            {
                //檢核權限
                X1TokenAuthCheckM.X1TokenAuthCheckReq x1TokenAuthCheckReq = new X1TokenAuthCheckM.X1TokenAuthCheckReq()
                {
                    AccID = request.AccID,
                    FuncCode = request.FuncCode,
                    AuthCode = request.AuthCode,
                    UserSecurityInfo = request.UserSecurityInfo
                };

                var checkRS = await _authHandler.CheckFuncAuth(request);
                if (checkRS.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, checkRS.ReturnCode, "(Auth)" + checkRS.ReturnMsg);
                }
                //若有更新Token需回傳
                if (checkRS.TokenChgFlag)
                {
                    retResp.TokenChgFlag = checkRS.TokenChgFlag;
                    retResp.UserSecurityInfo = checkRS.UserSecurityInfo;
                }

                // 查詢 Report
                string serverImagePath = System.Web.HttpContext.Current.Server.MapPath("~/Content/Temp");
                Directory.CreateDirectory(serverImagePath);
                var GetReportRS = _svc.GetReport(request, ref retResp, serverImagePath);
                if (GetReportRS.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, ErrorCode.ProcessError, GetReportRS.ReturnMsg);
                }

                // 處理回傳的檔案
                string host = UriUtility.GetBaseUrl(Request, "~/api/Report/GetReportAnsFile");

                for (int i = 0; i < retResp.Files.Count(); i++)
                {
                    retResp.Files[i].FileURL = host + "?ID=" + retResp.Files[i].FileURL;
                }

                return Result.NormalResult(request, retResp);
            }
            else
            {
                string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
                return Result.NormalResult(request, retResp, ErrorCode.ArgInvalid, errorMessage);
            }
        }

        /// <summary>
        /// 取得 Report 清單
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<GetReportsM.GetReportsRsp> GetReports(GetReportsM.GetReportsReq request)
        {
           
            GetReportsM.GetReportsRsp retResp = new GetReportsM.GetReportsRsp();

            if (ModelState.IsValid)
            {
                //檢核權限
                X1TokenAuthCheckM.X1TokenAuthCheckReq x1TokenAuthCheckReq = new X1TokenAuthCheckM.X1TokenAuthCheckReq()
                {
                    AccID = request.AccID,
                    FuncCode = request.FuncCode,
                    AuthCode = request.AuthCode,
                    UserSecurityInfo = request.UserSecurityInfo
                };

                var checkRS = await _authHandler.CheckFuncAuth(request);
                if (checkRS.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, checkRS.ReturnCode, "(Auth)" + checkRS.ReturnMsg);
                }
                //若有更新Token需回傳
                if (checkRS.TokenChgFlag)
                {
                    retResp.TokenChgFlag = checkRS.TokenChgFlag;
                    retResp.UserSecurityInfo = checkRS.UserSecurityInfo;
                }

                // 查詢 Report 清單
                var GetReportsRS = _svc.GetReports(request, ref retResp);
                if (GetReportsRS.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, ErrorCode.ProcessError, GetReportsRS.ReturnMsg);
                }

                return Result.NormalResult(request, retResp);
            }
            else
            {
                string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
                return Result.NormalResult(request, retResp, ErrorCode.ArgInvalid, errorMessage);
            }
        }

        /// <summary>
        /// 更新Report
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<UpdateReportM.UpdateReportRsp> UpdateReport(UpdateReportM.UpdateReportReq request)
        {
            UpdateReportM.UpdateReportRsp retResp = new UpdateReportM.UpdateReportRsp();

            if (ModelState.IsValid)
            {
                //檢核權限
                X1TokenAuthCheckM.X1TokenAuthCheckReq x1TokenAuthCheckReq = new X1TokenAuthCheckM.X1TokenAuthCheckReq()
                {
                    AccID = request.AccID,
                    FuncCode = request.FuncCode,
                    AuthCode = request.AuthCode,
                    UserSecurityInfo = request.UserSecurityInfo
                };

                var checkRS = await _authHandler.CheckFuncAuth(request);
                if (checkRS.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, checkRS.ReturnCode, "(Auth)" + checkRS.ReturnMsg);
                }
                //若有更新Token需回傳
                if (checkRS.TokenChgFlag)
                {
                    retResp.TokenChgFlag = checkRS.TokenChgFlag;
                    retResp.UserSecurityInfo = checkRS.UserSecurityInfo;
                }

                // 查詢 Report 清單
                var UpdateReporRS = _svc.UpdateReport(request, ref retResp);
                if (UpdateReporRS.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, ErrorCode.ProcessError, UpdateReporRS.ReturnMsg);
                }

                return Result.NormalResult(request, retResp);
            }
            else
            {
                string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
                return Result.NormalResult(request, retResp, ErrorCode.ArgInvalid, errorMessage);
            }
        }

        /// <summary>
        /// 更新 Report 檔案
        /// </summary>
        /// <returns></returns>
        [IgnoreRequestContent]
        public async Task<UpdateReportFileM.UpdateReportFileRsp> UpdateReportFile()
        {
            var request = new UpdateReportFileM.UpdateReportFileReq();
            UpdateReportFileM.UpdateReportFileRsp retResp = new UpdateReportFileM.UpdateReportFileRsp();

            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = HttpContext.Current.Server.MapPath("~/App_Data/Temp");
            Directory.CreateDirectory(root);
            var provider = new MultipartFormDataStreamProvider(root);

            await Request.Content.ReadAsMultipartAsync(provider);

            try
            {
                request.AccID = provider.FormData["AccID"];
                request.FuncCode = provider.FormData["FuncCode"];
                request.AuthCode = int.Parse(provider.FormData["AuthCode"]);
                request.UserSecurityInfo = provider.FormData["UserSecurityInfo"];
                request.ReportID = int.Parse(provider.FormData["ReportID"]);
                request.QuestionID = provider.FormData["QuestionID"];
                request.FilePath = provider.FileData[0].LocalFileName.Replace("\"", "");
                request.FileName = provider.FileData[0].Headers.ContentDisposition.FileName.Replace("\"", "");
                request.MimeType = provider.FileData[0].Headers.ContentType.MediaType.Replace("\"", "");
            }
            catch (Exception e)
            {
                return Result.NormalResult(request, retResp, ErrorCode.ArgInvalid, e.Message);
            }

            //檢核權限
            var checkRS = await _authHandler.CheckFuncAuth(request);
            if (checkRS.ReturnCode != ErrorCode.OK)
            {
                return Result.NormalResult(request, retResp, checkRS.ReturnCode, "(Auth)" + checkRS.ReturnMsg);
            }
            //若有更新Token需回傳
            if (checkRS.TokenChgFlag)
            {
                retResp.TokenChgFlag = checkRS.TokenChgFlag;
                retResp.UserSecurityInfo = checkRS.UserSecurityInfo;
            }

            // 查詢 Report 清單
            var UpdateReportFile = _svc.UpdateReportFile(request, ref retResp);
            if (UpdateReportFile.ReturnCode != ErrorCode.OK)
            {
                return Result.NormalResult(request, retResp, ErrorCode.ProcessError, UpdateReportFile.ReturnMsg);
            }

            return Result.NormalResult(request, retResp);
        }

        /// <summary>
        /// 新增一般 Report
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<AddGeneralReportM.AddGeneralReportRsp> AddGeneralReport(AddGeneralReportM.AddGeneralReportReq request)
        {

            AddGeneralReportM.AddGeneralReportRsp retResp = new AddGeneralReportM.AddGeneralReportRsp();

            if (ModelState.IsValid)
            {
                //檢核權限
                X1TokenAuthCheckM.X1TokenAuthCheckReq x1TokenAuthCheckReq = new X1TokenAuthCheckM.X1TokenAuthCheckReq()
                {
                    AccID = request.AccID,
                    FuncCode = request.FuncCode,
                    AuthCode = request.AuthCode,
                    UserSecurityInfo = request.UserSecurityInfo
                };

                var checkRS = await _authHandler.CheckFuncAuth(request);
                if (checkRS.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, checkRS.ReturnCode, "(Auth)" + checkRS.ReturnMsg);
                }
                //若有更新Token需回傳
                if (checkRS.TokenChgFlag)
                {
                    retResp.TokenChgFlag = checkRS.TokenChgFlag;
                    retResp.UserSecurityInfo = checkRS.UserSecurityInfo;
                }

                // 新增 Report
                var AddGeneralReportRS = _svc.AddGeneralReport(request, ref retResp);
                if (AddGeneralReportRS.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, ErrorCode.ProcessError, AddGeneralReportRS.ReturnMsg);
                }

                return Result.NormalResult(request, retResp);
            }
            else
            {
                string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
                return Result.NormalResult(request, retResp, ErrorCode.ArgInvalid, errorMessage);
            }
        }

        /// <summary>
        /// 匯出PDF
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ExportReportM.ExportReportRsp> ExportReport(ExportReportM.ExportReportReq request)
        {

            ExportReportM.ExportReportRsp retResp = new ExportReportM.ExportReportRsp();

            if (ModelState.IsValid)
            {
                //檢核權限
                X1TokenAuthCheckM.X1TokenAuthCheckReq x1TokenAuthCheckReq = new X1TokenAuthCheckM.X1TokenAuthCheckReq()
                {
                    AccID = request.AccID,
                    FuncCode = request.FuncCode,
                    AuthCode = request.AuthCode,
                    UserSecurityInfo = request.UserSecurityInfo
                };

                var checkRS = await _authHandler.CheckFuncAuth(x1TokenAuthCheckReq);
                if (checkRS.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, checkRS.ReturnCode, "(Auth)" + checkRS.ReturnMsg);
                }
                //若有更新Token需回傳
                if (checkRS.TokenChgFlag)
                {
                    retResp.TokenChgFlag = checkRS.TokenChgFlag;
                    retResp.UserSecurityInfo = checkRS.UserSecurityInfo;
                }

                // 匯出 Report
                string rootPath = System.Web.Hosting.HostingEnvironment.MapPath("~");
                var ExportReportRS = _svc.ExportReport(request, ref retResp, rootPath);
                if (ExportReportRS.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, ErrorCode.ProcessError, ExportReportRS.ReturnMsg);
                }

                retResp.pdfURL = Request.GetRequestContext().VirtualPathRoot + "/ReportPDF/" + retResp.pdfURL;

                return Result.NormalResult(request, retResp);
            }
            else
            {
                string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
                return Result.NormalResult(request, retResp, ErrorCode.ArgInvalid, errorMessage);
            }
        }

        /// <summary>
        /// 取得Report診斷、檢體、醫令資料
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //[HttpPost]
        //public GetReportInfoM.GetReportInfoRsp GetReportInfo(GetReportInfoM.GetReportInfoReq request)
        //{

        //    GetReportInfoM.GetReportInfoRsp retResp = new GetReportInfoM.GetReportInfoRsp();

        //    if (ModelState.IsValid)
        //    {
        //        //檢核權限
        //        X1TokenAuthCheckM.X1TokenAuthCheckReq x1TokenAuthCheckReq = new X1TokenAuthCheckM.X1TokenAuthCheckReq()
        //        {
        //            AccID = request.AccID,
        //            FuncCode = request.FuncCode,
        //            AuthCode = request.AuthCode,
        //            UserSecurityInfo = request.UserSecurityInfo
        //        };

        //        var checkRS = _authHandler.CheckFuncAuth(request);
        //        if (checkRS.ReturnCode != ErrorCode.OK)
        //        {
        //            return Result.NormalResult(request, retResp, checkRS.ReturnCode, "(Auth)" + checkRS.ReturnMsg);
        //        }
        //        //若有更新Token需回傳
        //        if (checkRS.TokenChgFlag)
        //        {
        //            retResp.TokenChgFlag = checkRS.TokenChgFlag;
        //            retResp.UserSecurityInfo = checkRS.UserSecurityInfo;
        //        }

        //        // 取得資料
        //        var getReportInfoRS = _svc.GetReportInfo(request, ref retResp);
        //        if (getReportInfoRS.ReturnCode != ErrorCode.OK)
        //        {
        //            return Result.NormalResult(request, retResp, ErrorCode.ProcessError, getReportInfoRS.ReturnMsg);
        //        }

        //        return Result.NormalResult(request, retResp, ErrorCode.OK, "OK");
        //    }
        //    else
        //    {
        //        string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
        //        return Result.NormalResult(request, retResp, ErrorCode.ArgInvalid, errorMessage);
        //    }
        //}

        /// <summary>
        /// 更新檢體資料
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //[HttpPost]
        //public UpdateSpecimenM.UpdateSpecimenRsp UpdateSpecimen(UpdateSpecimenM.UpdateSpecimenReq request)
        //{
        //    Trace.WriteLine("UpdateSpecimen Guid: " + guid.ToString());
        //    UpdateSpecimenM.UpdateSpecimenRsp retResp = new UpdateSpecimenM.UpdateSpecimenRsp();

        //    if (ModelState.IsValid)
        //    {
        //        //檢核權限
        //        var checkRS = _authHandler.CheckFuncAuth(request);
        //        if (checkRS.ReturnCode != ErrorCode.OK)
        //        {
        //            return Result.NormalResult(request, retResp, checkRS.ReturnCode, "(Auth)" + checkRS.ReturnMsg);
        //        }
        //        //若有更新Token需回傳
        //        if (checkRS.TokenChgFlag)
        //        {
        //            retResp.TokenChgFlag = checkRS.TokenChgFlag;
        //            retResp.UserSecurityInfo = checkRS.UserSecurityInfo;
        //        }

        //        // 取得資料
        //        var serviceResponse = _svc.UpdateSpecimen(request, ref retResp);
        //        if (serviceResponse.ReturnCode != ErrorCode.OK)
        //        {
        //            return Result.NormalResult(request, retResp, ErrorCode.ProcessError, serviceResponse.ReturnMsg);
        //        }

        //        return Result.NormalResult(request, retResp);
        //    }
        //    else
        //    {
        //        string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
        //        return Result.NormalResult(request, retResp, ErrorCode.ArgInvalid, errorMessage);
        //    }
        //}

        /// <summary>
        /// 取得匯出Report清單
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<GetExportReportListLazyM.GetExportReportListLazyRsp> GetExportReportListLazy(GetExportReportListLazyM.GetExportReportListLazyReq request)
        {
            GetExportReportListLazyM.GetExportReportListLazyRsp retResp = new GetExportReportListLazyM.GetExportReportListLazyRsp();

            if (ModelState.IsValid)
            {
                //檢核權限
                var checkRS = await _authHandler.CheckFuncAuth(request);
                if (checkRS.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, checkRS.ReturnCode, "(Auth)" + checkRS.ReturnMsg);
                }
                //若有更新Token需回傳
                if (checkRS.TokenChgFlag)
                {
                    retResp.TokenChgFlag = checkRS.TokenChgFlag;
                    retResp.UserSecurityInfo = checkRS.UserSecurityInfo;
                }

                // 取得資料
                var serviceResponse = _svc.GetExportReportListLazy(request, ref retResp);
                if (serviceResponse.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, ErrorCode.ProcessError, serviceResponse.ReturnMsg);
                }

                return Result.NormalResult(request, retResp);
            }
            else
            {
                string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
                return Result.NormalResult(request, retResp, ErrorCode.ArgInvalid, errorMessage);
            }
        }

        /// <summary>
        /// 取得Panel對應的資料
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //[HttpPost]
        //public GetPanelDataM.GetPanelDataRsp GetPanelData(GetPanelDataM.GetPanelDataReq request)
        //{
        //    GetPanelDataM.GetPanelDataRsp retResp = new GetPanelDataM.GetPanelDataRsp();

        //    if (ModelState.IsValid)
        //    {
        //        //檢核權限
        //        var checkRS = _authHandler.CheckFuncAuth(request);
        //        if (checkRS.ReturnCode != ErrorCode.OK)
        //        {
        //            return Result.NormalResult(request, retResp, checkRS.ReturnCode, "(Auth)" + checkRS.ReturnMsg);
        //        }
        //        //若有更新Token需回傳
        //        if (checkRS.TokenChgFlag)
        //        {
        //            retResp.TokenChgFlag = checkRS.TokenChgFlag;
        //            retResp.UserSecurityInfo = checkRS.UserSecurityInfo;
        //        }

        //        // 取得資料
        //        var serviceResponse = _svc.GetPanelData(request, ref retResp);
        //        if (serviceResponse.ReturnCode != ErrorCode.OK)
        //        {
        //            return Result.NormalResult(request, retResp, ErrorCode.ProcessError, serviceResponse.ReturnMsg);
        //        }

        //        return Result.NormalResult(request, retResp);
        //    }
        //    else
        //    {
        //        string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
        //        return Result.NormalResult(request, retResp, ErrorCode.ArgInvalid, errorMessage);
        //    }
        //}

        /// <summary>
        /// 取得Panel對應的抗體及細胞類別
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //[HttpPost]
        //public GetPanelMappingIgM.GetPanelMappingIgRsp GetPanelMappingIg(GetPanelMappingIgM.GetPanelMappingIgReq request)
        //{
        //    GetPanelMappingIgM.GetPanelMappingIgRsp retResp = new GetPanelMappingIgM.GetPanelMappingIgRsp();

        //    if (ModelState.IsValid)
        //    {
        //        //檢核權限
        //        var checkRS = _authHandler.CheckFuncAuth(request);
        //        if (checkRS.ReturnCode != ErrorCode.OK)
        //        {
        //            return Result.NormalResult(request, retResp, checkRS.ReturnCode, "(Auth)" + checkRS.ReturnMsg);
        //        }
        //        //若有更新Token需回傳
        //        if (checkRS.TokenChgFlag)
        //        {
        //            retResp.TokenChgFlag = checkRS.TokenChgFlag;
        //            retResp.UserSecurityInfo = checkRS.UserSecurityInfo;
        //        }

        //        // 取得資料
        //        var serviceResponse = _svc.GetPanelMappingIg(request, ref retResp);
        //        if (serviceResponse.ReturnCode != ErrorCode.OK)
        //        {
        //            return Result.NormalResult(request, retResp, ErrorCode.ProcessError, serviceResponse.ReturnMsg);
        //        }

        //        return Result.NormalResult(request, retResp);
        //    }
        //    else
        //    {
        //        string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
        //        return Result.NormalResult(request, retResp, ErrorCode.ArgInvalid, errorMessage);
        //    }
        //}

        /// <summary>
        /// 取得問卷清單
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<GetQuestionListM.GetQuestionListRsp> GetQuestionList(GetQuestionListM.GetQuestionListReq request)
        {
            GetQuestionListM.GetQuestionListRsp retResp = new GetQuestionListM.GetQuestionListRsp();

            if (ModelState.IsValid)
            {
                //檢核權限
                var checkRS = await _authHandler.CheckFuncAuth(request);
                if (checkRS.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, checkRS.ReturnCode, "(Auth)" + checkRS.ReturnMsg);
                }
                //若有更新Token需回傳
                if (checkRS.TokenChgFlag)
                {
                    retResp.TokenChgFlag = checkRS.TokenChgFlag;
                    retResp.UserSecurityInfo = checkRS.UserSecurityInfo;
                }

                // 取得資料
                var serviceResponse = _svc.GetQuestionList(request, ref retResp);
                if (serviceResponse.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, ErrorCode.ProcessError, serviceResponse.ReturnMsg);
                }

                return Result.NormalResult(request, retResp);
            }
            else
            {
                string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
                return Result.NormalResult(request, retResp, ErrorCode.ArgInvalid, errorMessage);
            }
        }
        /// <summary>
        /// 取得罐頭語選項
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //[HttpPost]
        //public GetTextTemplateM.GetTextTemplateRsp GetTextTemplate(GetTextTemplateM.GetTextTemplateReq request)
        //{
        //    GetTextTemplateM.GetTextTemplateRsp retResp = new GetTextTemplateM.GetTextTemplateRsp();

        //    if (ModelState.IsValid)
        //    {
        //        //檢核權限
        //        var checkRS = _authHandler.CheckFuncAuth(request);
        //        if (checkRS.ReturnCode != ErrorCode.OK)
        //        {
        //            return Result.NormalResult(request, retResp, checkRS.ReturnCode, "(Auth)" + checkRS.ReturnMsg);
        //        }
        //        //若有更新Token需回傳
        //        if (checkRS.TokenChgFlag)
        //        {
        //            retResp.TokenChgFlag = checkRS.TokenChgFlag;
        //            retResp.UserSecurityInfo = checkRS.UserSecurityInfo;
        //        }

        //        // 取得資料
        //        var serviceResponse = _svc.GetTextTemplate(request, ref retResp);
        //        if (serviceResponse.ReturnCode != ErrorCode.OK)
        //        {
        //            return Result.NormalResult(request, retResp, ErrorCode.ProcessError, serviceResponse.ReturnMsg);
        //        }

        //        return Result.NormalResult(request, retResp);
        //    }
        //    else
        //    {
        //        string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
        //        return Result.NormalResult(request, retResp, ErrorCode.ArgInvalid, errorMessage);
        //    }
        //}

        /// <summary>
        /// 取得 Panel 對應的罐頭語選項
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //[HttpPost]
        //public GetPanelTextTemplateM.GetPanelTextTemplateRsp GetPanelTextTemplate(GetPanelTextTemplateM.GetPanelTextTemplateReq request)
        //{
        //    GetPanelTextTemplateM.GetPanelTextTemplateRsp retResp = new GetPanelTextTemplateM.GetPanelTextTemplateRsp();

        //    if (ModelState.IsValid)
        //    {
        //        //檢核權限
        //        var checkRS = _authHandler.CheckFuncAuth(request);
        //        if (checkRS.ReturnCode != ErrorCode.OK)
        //        {
        //            return Result.NormalResult(request, retResp, checkRS.ReturnCode, "(Auth)" + checkRS.ReturnMsg);
        //        }
        //        //若有更新Token需回傳
        //        if (checkRS.TokenChgFlag)
        //        {
        //            retResp.TokenChgFlag = checkRS.TokenChgFlag;
        //            retResp.UserSecurityInfo = checkRS.UserSecurityInfo;
        //        }

        //        // 取得資料
        //        var serviceResponse = _svc.GetPanelTextTemplate(request, ref retResp);
        //        if (serviceResponse.ReturnCode != ErrorCode.OK)
        //        {
        //            return Result.NormalResult(request, retResp, ErrorCode.ProcessError, serviceResponse.ReturnMsg);
        //        }

        //        return Result.NormalResult(request, retResp);
        //    }
        //    else
        //    {
        //        string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
        //        return Result.NormalResult(request, retResp, ErrorCode.ArgInvalid, errorMessage);
        //    }
        //}

        /// <summary>
        /// 取得 Convention 最近的匯出時間
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<GetLatestCBCExportDateM.GetLatestCBCExportDateRsp> GetLatestCBCExportDate(GetLatestCBCExportDateM.GetLatestCBCExportDateReq request)
        {
            var retResp = new GetLatestCBCExportDateM.GetLatestCBCExportDateRsp();

            if (ModelState.IsValid)
            {
                //檢核權限
                var checkRS = await _authHandler.CheckFuncAuth(request);
                if (checkRS.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, checkRS.ReturnCode, "(Auth)" + checkRS.ReturnMsg);
                }
                //若有更新Token需回傳
                if (checkRS.TokenChgFlag)
                {
                    retResp.TokenChgFlag = checkRS.TokenChgFlag;
                    retResp.UserSecurityInfo = checkRS.UserSecurityInfo;
                }

                // 取得資料
                var serviceResponse = _svc.GetLatestCBCExportDate(request, ref retResp);
                if (serviceResponse.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, ErrorCode.ProcessError, serviceResponse.ReturnMsg);
                }

                return Result.NormalResult(request, retResp);
            }
            else
            {
                string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
                return Result.NormalResult(request, retResp, ErrorCode.ArgInvalid, errorMessage);
            }
        }

        /// <summary>
        /// 解鎖Report
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<UnlockReportM.UnlockReportRsp> UnlockReport(UnlockReportM.UnlockReportReq request)
        {
            var retResp = new UnlockReportM.UnlockReportRsp();

            if (ModelState.IsValid)
            {
                //檢核權限
                var checkRS = await _authHandler.CheckFuncAuth(request);
                if (checkRS.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, checkRS.ReturnCode, "(Auth)" + checkRS.ReturnMsg);
                }
                //若有更新Token需回傳
                if (checkRS.TokenChgFlag)
                {
                    retResp.TokenChgFlag = checkRS.TokenChgFlag;
                    retResp.UserSecurityInfo = checkRS.UserSecurityInfo;
                }

                // 解鎖 Report
                var serviceResponse = _svc.UnlockReport(request, ref retResp);
                if (serviceResponse.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, ErrorCode.ProcessError, serviceResponse.ReturnMsg);
                }

                return Result.NormalResult(request, retResp);
            }
            else
            {
                string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
                return Result.NormalResult(request, retResp, ErrorCode.ArgInvalid, errorMessage);
            }
        }

        /// <summary>
        /// 結案Report
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<CloseoutReportM.CloseoutReportRsp> CloseoutReport(CloseoutReportM.CloseoutReportReq request)
        {
            var retResp = new CloseoutReportM.CloseoutReportRsp();

            if (ModelState.IsValid)
            {
                //檢核權限
                var checkRS = await _authHandler.CheckFuncAuth(request);
                if (checkRS.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, checkRS.ReturnCode, "(Auth)" + checkRS.ReturnMsg);
                }
                //若有更新Token需回傳
                if (checkRS.TokenChgFlag)
                {
                    retResp.TokenChgFlag = checkRS.TokenChgFlag;
                    retResp.UserSecurityInfo = checkRS.UserSecurityInfo;
                }

                // 解鎖 Report
                var serviceResponse = _svc.CloseoutReport(request, ref retResp);
                if (serviceResponse.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, ErrorCode.ProcessError, serviceResponse.ReturnMsg);
                }

                return Result.NormalResult(request, retResp);
            }
            else
            {
                string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
                return Result.NormalResult(request, retResp, ErrorCode.ArgInvalid, errorMessage);
            }
        }

        /// <summary>
        /// 取得所有Report Main
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<GetAllReportMainM.GetAllReportMainRsp> GetAllReportMain(GetAllReportMainM.GetAllReportMainReq request)
        {
            var retResp = new GetAllReportMainM.GetAllReportMainRsp();

            if (ModelState.IsValid)
            {
                //檢核權限
                var checkRS = await _authHandler.CheckFuncAuth(request);
                if (checkRS.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, checkRS.ReturnCode, "(Auth)" + checkRS.ReturnMsg);
                }
                //若有更新Token需回傳
                if (checkRS.TokenChgFlag)
                {
                    retResp.TokenChgFlag = checkRS.TokenChgFlag;
                    retResp.UserSecurityInfo = checkRS.UserSecurityInfo;
                }

                // 取得所有 Report Main
                var serviceResponse = _svc.GetAllReportMain(request, ref retResp);
                if (serviceResponse.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, ErrorCode.ProcessError, serviceResponse.ReturnMsg);
                }

                return Result.NormalResult(request, retResp);
            }
            else
            {
                string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
                return Result.NormalResult(request, retResp, ErrorCode.ArgInvalid, errorMessage);
            }
        }

        /// <summary>
        /// 取得 Report Main 資料
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<GetReportMainM.GetReportMainRsp> GetReportMain(GetReportMainM.GetReportMainReq request)
        {
            var retResp = new GetReportMainM.GetReportMainRsp();

            if (ModelState.IsValid)
            {
                //檢核權限
                var checkRS = await _authHandler.CheckFuncAuth(request);
                if (checkRS.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, checkRS.ReturnCode, "(Auth)" + checkRS.ReturnMsg);
                }
                //若有更新Token需回傳
                if (checkRS.TokenChgFlag)
                {
                    retResp.TokenChgFlag = checkRS.TokenChgFlag;
                    retResp.UserSecurityInfo = checkRS.UserSecurityInfo;
                }

                // 取得 Report Main
                var serviceResponse = _svc.GetReportMain(request, ref retResp);
                if (serviceResponse.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, ErrorCode.ProcessError, serviceResponse.ReturnMsg);
                }

                return Result.NormalResult(request, retResp);
            }
            else
            {
                string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
                return Result.NormalResult(request, retResp, ErrorCode.ArgInvalid, errorMessage);
            }
        }

        /// <summary>
        /// 更新 Report Main 資料
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<UpdateReportMainM.UpdateReportMainRsp> UpdateReportMain(UpdateReportMainM.UpdateReportMainReq request)
        {
            var retResp = new UpdateReportMainM.UpdateReportMainRsp();

            if (ModelState.IsValid)
            {
                //檢核權限
                var checkRS = await _authHandler.CheckFuncAuth(request);
                if (checkRS.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, checkRS.ReturnCode, "(Auth)" + checkRS.ReturnMsg);
                }
                //若有更新Token需回傳
                if (checkRS.TokenChgFlag)
                {
                    retResp.TokenChgFlag = checkRS.TokenChgFlag;
                    retResp.UserSecurityInfo = checkRS.UserSecurityInfo;
                }

                // 取得 Report Main
                var serviceResponse = _svc.UpdateReportMain(request, ref retResp);
                if (serviceResponse.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, ErrorCode.ProcessError, serviceResponse.ReturnMsg);
                }

                return Result.NormalResult(request, retResp);
            }
            else
            {
                string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
                return Result.NormalResult(request, retResp, ErrorCode.ArgInvalid, errorMessage);
            }
        }

        /// <summary>
        /// 更新檢驗單狀態
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<UpdateReportStatusM.UpdateReportStatusRsp> UpdateReportStatus(UpdateReportStatusM.UpdateReportStatusReq request)
        {
            var retResp = new UpdateReportStatusM.UpdateReportStatusRsp();

            if (ModelState.IsValid)
            {
                //檢核權限
                var checkRS = await _authHandler.CheckFuncAuth(request);
                if (checkRS.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, checkRS.ReturnCode, "(Auth)" + checkRS.ReturnMsg);
                }
                //若有更新Token需回傳
                if (checkRS.TokenChgFlag)
                {
                    retResp.TokenChgFlag = checkRS.TokenChgFlag;
                    retResp.UserSecurityInfo = checkRS.UserSecurityInfo;
                }

                //查詢待處理清單
                var serviceResponse = _svc.UpdateReportStatus(request);
                if (serviceResponse.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, ErrorCode.ProcessError, serviceResponse.ReturnMsg);
                }

                return Result.NormalResult(request, retResp);
            }
            else
            {
                string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
                return Result.NormalResult(request, retResp, ErrorCode.ArgInvalid, errorMessage);
            }
        }

        /// <summary>
        /// 新增 Report Main 資料
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<AddReportMainM.AddReportMainRsp> AddReportMain(AddReportMainM.AddReportMainReq request)
        {
            var retResp = new AddReportMainM.AddReportMainRsp();

            if (ModelState.IsValid)
            {
                //檢核權限
                var checkRS = await _authHandler.CheckFuncAuth(request);
                if (checkRS.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, checkRS.ReturnCode, "(Auth)" + checkRS.ReturnMsg);
                }
                //若有更新Token需回傳
                if (checkRS.TokenChgFlag)
                {
                    retResp.TokenChgFlag = checkRS.TokenChgFlag;
                    retResp.UserSecurityInfo = checkRS.UserSecurityInfo;
                }

                var dbname = _frameReq.GetDBName();
                // 新增 Report Main
                var serviceResponse = _svc.AddReportMain(request, ref retResp, dbname);
                if (serviceResponse.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, ErrorCode.ProcessError, serviceResponse.ReturnMsg);
                }

                return Result.NormalResult(request, retResp);
            }
            else
            {
                string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
                return Result.NormalResult(request, retResp, ErrorCode.ArgInvalid, errorMessage);
            }
        }

        /// <summary>
        /// 刪除Report Main
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<DeleteReportMainM.DeleteReportMainRsp> DeleteReportMain(DeleteReportMainM.DeleteReportMainReq request)
        {
            var retResp = new DeleteReportMainM.DeleteReportMainRsp();

            if (ModelState.IsValid)
            {
                //檢核權限
                var checkRS = await _authHandler.CheckFuncAuth(request);
                if (checkRS.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, checkRS.ReturnCode, "(Auth)" + checkRS.ReturnMsg);
                }
                //若有更新Token需回傳
                if (checkRS.TokenChgFlag)
                {
                    retResp.TokenChgFlag = checkRS.TokenChgFlag;
                    retResp.UserSecurityInfo = checkRS.UserSecurityInfo;
                }

                // 刪除 Report Main
                var serviceResponse = _svc.DeleteReportMain(request, ref retResp);
                if (serviceResponse.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, ErrorCode.ProcessError, serviceResponse.ReturnMsg);
                }

                return Result.NormalResult(request, retResp);
            }
            else
            {
                string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
                return Result.NormalResult(request, retResp, ErrorCode.ArgInvalid, errorMessage);
            }
        }

        /// <summary>
        /// 發佈 Report
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PublishReportM.PublishReportRsp> PublishReport(PublishReportM.PublishReportReq request)
        {
            var retResp = new PublishReportM.PublishReportRsp();

            if (ModelState.IsValid)
            {
                //檢核權限
                var checkRS = await _authHandler.CheckFuncAuth(request);
                if (checkRS.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, checkRS.ReturnCode, "(Auth)" + checkRS.ReturnMsg);
                }
                //若有更新Token需回傳
                if (checkRS.TokenChgFlag)
                {
                    retResp.TokenChgFlag = checkRS.TokenChgFlag;
                    retResp.UserSecurityInfo = checkRS.UserSecurityInfo;
                }

                // 發佈 Report
                var serviceResponse = _svc.PublishReport(request, ref retResp);
                if (serviceResponse.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, ErrorCode.ProcessError, serviceResponse.ReturnMsg);
                }

                return Result.NormalResult(request, retResp);
            }
            else
            {
                string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
                return Result.NormalResult(request, retResp, ErrorCode.ArgInvalid, errorMessage);
            }
        }

        /// <summary>
        /// 取得所有版本的Report
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<GetAllVerionReportM.GetAllVersionReportRsp> GetAllVersionReport(GetAllVerionReportM.GetAllVersionReportReq request)
        {
            var retResp = new GetAllVerionReportM.GetAllVersionReportRsp();

            if (ModelState.IsValid)
            {
                //檢核權限
                var checkRS = await _authHandler.CheckFuncAuth(request);
                if (checkRS.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, checkRS.ReturnCode, "(Auth)" + checkRS.ReturnMsg);
                }
                //若有更新Token需回傳
                if (checkRS.TokenChgFlag)
                {
                    retResp.TokenChgFlag = checkRS.TokenChgFlag;
                    retResp.UserSecurityInfo = checkRS.UserSecurityInfo;
                }

                // 取得所有版本的Report
                var serviceResponse = _svc.GetAllVersionReport(request, ref retResp);
                if (serviceResponse.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, ErrorCode.ProcessError, serviceResponse.ReturnMsg);
                }

                return Result.NormalResult(request, retResp);
            }
            else
            {
                string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
                return Result.NormalResult(request, retResp, ErrorCode.ArgInvalid, errorMessage);
            }
        }

        /// <summary>
        /// 上傳 Report Main 圖片
        /// </summary>
        /// <returns></returns>
        [IgnoreRequestContent]
        public async Task<AddReportMainFileM.AddReportMainFileRsp> AddReportMainFile()
        {
            var request = new AddReportMainFileM.AddReportMainFileReq();
            var retResp = new AddReportMainFileM.AddReportMainFileRsp();

            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = HttpContext.Current.Server.MapPath("~/App_Data/Temp");
            Directory.CreateDirectory(root);
            var provider = new MultipartFormDataStreamProvider(root);

            await Request.Content.ReadAsMultipartAsync(provider);

            try
            {
                request.AccID = provider.FormData["AccID"];
                request.FuncCode = provider.FormData["FuncCode"];
                request.AuthCode = int.Parse(provider.FormData["AuthCode"]);
                request.UserSecurityInfo = provider.FormData["UserSecurityInfo"];
                request.RQID = int.Parse(provider.FormData["RQID"]);
                request.FilePath = provider.FileData[0].LocalFileName.Replace("\"", "");
                request.FileName = provider.FileData[0].Headers.ContentDisposition.FileName.Replace("\"", "");
                request.MimeType = provider.FileData[0].Headers.ContentType.MediaType.Replace("\"", "");
            }
            catch (Exception e)
            {
                return Result.NormalResult(request, retResp, ErrorCode.ArgInvalid, e.Message);
            }

            //檢核權限
            var checkRS = await _authHandler.CheckFuncAuth(request);
            if (checkRS.ReturnCode != ErrorCode.OK)
            {
                return Result.NormalResult(request, retResp, checkRS.ReturnCode, "(Auth)" + checkRS.ReturnMsg);
            }
            //若有更新Token需回傳
            if (checkRS.TokenChgFlag)
            {
                retResp.TokenChgFlag = checkRS.TokenChgFlag;
                retResp.UserSecurityInfo = checkRS.UserSecurityInfo;
            }

            // 新增問題檔案
            var serviceRsp = _svc.AddReportMainFile(request, ref retResp);
            if (serviceRsp.ReturnCode != ErrorCode.OK)
            {
                return Result.NormalResult(request, retResp, ErrorCode.ProcessError, serviceRsp.ReturnMsg);
            }

            return Result.NormalResult(request, retResp);
        }

        /// <summary>
        /// 上傳 Report Answer 檔案
        /// </summary>
        /// <returns></returns>
        [IgnoreRequestContent]
        public async Task<AddReportAnsFileM.AddReportAnsFileRsp> AddReportAnsFile()
        {
            var request = new AddReportAnsFileM.AddReportAnsFileReq();
            var retResp = new AddReportAnsFileM.AddReportAnsFileRsp();

            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = HttpContext.Current.Server.MapPath("~/App_Data/Temp");
            Directory.CreateDirectory(root);
            var provider = new MultipartFormDataStreamProvider(root);

            await Request.Content.ReadAsMultipartAsync(provider);

            try
            {
                request.AccID = provider.FormData["AccID"];
                request.FuncCode = provider.FormData["FuncCode"];
                request.AuthCode = int.Parse(provider.FormData["AuthCode"]);
                request.UserSecurityInfo = provider.FormData["UserSecurityInfo"];
                request.AnswerMID = int.Parse(provider.FormData["AnswerMID"]);
                request.QuestionID = int.Parse(provider.FormData["QuestionID"]);
                request.FilePath = provider.FileData[0].LocalFileName.Replace("\"", "");
                request.FileName = provider.FileData[0].Headers.ContentDisposition.FileName.Replace("\"", "");
                request.MimeType = provider.FileData[0].Headers.ContentType.MediaType.Replace("\"", "");
            }
            catch (Exception e)
            {
                return Result.NormalResult(request, retResp, ErrorCode.ArgInvalid, e.Message);
            }

            //檢核權限
            var checkRS = await _authHandler.CheckFuncAuth(request);
            if (checkRS.ReturnCode != ErrorCode.OK)
            {
                return Result.NormalResult(request, retResp, checkRS.ReturnCode, "(Auth)" + checkRS.ReturnMsg);
            }
            //若有更新Token需回傳
            if (checkRS.TokenChgFlag)
            {
                retResp.TokenChgFlag = checkRS.TokenChgFlag;
                retResp.UserSecurityInfo = checkRS.UserSecurityInfo;
            }

            // 新增問題檔案
            var serviceRsp = _svc.AddReportAnsFile(request, ref retResp);
            if (serviceRsp.ReturnCode != ErrorCode.OK)
            {
                return Result.NormalResult(request, retResp, ErrorCode.ProcessError, serviceRsp.ReturnMsg);
            }

            return Result.NormalResult(request, retResp);
        }

        /// <summary>
        /// 取得 Report Main 檔案
        /// </summary>
        /// <param name="RQID"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [IgnoreResponseContent]
        [HttpGet]
        public IHttpActionResult GetReportMainFile(int RQID, string fileName)
        {
            var file = _svc.GetReportMainFile(RQID, fileName);
            if (file == null)
            {
                return NotFound();
            }

            var cdhv = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            cdhv.FileName = fileName;
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new ByteArrayContent(file.FileData);
            result.Content.Headers.ContentDisposition = cdhv;
            result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.MimeType);

            return ResponseMessage(result);
        }

        /// <summary>
        /// 取得 Report Answer 檔案
        /// </summary>
        /// <param name="RQID"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [IgnoreResponseContent]
        [HttpGet]
        public IHttpActionResult GetReportAnsFile(int ID)
        {
            var file = _svc.GetReportAnsFile(ID);
            if (file == null)
            {
                return NotFound();
            }

            var fileStream = new FileStream(file.FilePath, FileMode.Open, FileAccess.Read);
            var cdhv = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            cdhv.FileName = file.FileName;
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StreamContent(fileStream);
            result.Content.Headers.ContentDisposition = cdhv;
            result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.MimeType);
            result.Headers.Add("FileName", HttpUtility.UrlEncode(file.FileName));

            return ResponseMessage(result);
        }

        /// <summary>
        /// 匯出 Coding Book
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        //[IgnoreResponseContent]
        //[HttpGet]
        //public IHttpActionResult ExportCodingBook(string ID, bool isPublish = false)
        //{
        //    var request = new ExportCodingBookM.ExportCodingBookReq()
        //    {
        //        ReportMID = ID,
        //        IsPublish = isPublish
        //    };
        //    var response = new ExportCodingBookM.ExportCodingBookRsp();
        //    var path = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/Temp");
        //    var baseRsp = _svc.ExportCodingBook(request, ref response, path);
        //    if (baseRsp.ReturnCode != ErrorCode.OK)
        //    {
        //        return NotFound();
        //    }

        //    var cdhv = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
        //    cdhv.FileName = response.FileName;
        //    var result = new HttpResponseMessage(HttpStatusCode.OK);
        //    result.Content = new ByteArrayContent(File.ReadAllBytes(response.FilePath));
        //    result.Content.Headers.ContentDisposition = cdhv;
        //    result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/vnd.ms-excel");

        //    return ResponseMessage(result);
        //}
        [HttpPost]
        public async Task<ExportCodingBookM.ExportCodingBookRsp> ExportCodingBook(ExportCodingBookM.ExportCodingBookReq request)
        {
            var retResp = new ExportCodingBookM.ExportCodingBookRsp();

            if (ModelState.IsValid)
            {
                //檢核權限
                X1TokenAuthCheckM.X1TokenAuthCheckReq x1TokenAuthCheckReq = new X1TokenAuthCheckM.X1TokenAuthCheckReq()
                {
                    AccID = request.AccID,
                    FuncCode = request.FuncCode,
                    AuthCode = request.AuthCode,
                    UserSecurityInfo = request.UserSecurityInfo
                };

                var checkRS = await _authHandler.CheckFuncAuth(x1TokenAuthCheckReq);
                if (checkRS.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, checkRS.ReturnCode, "(Auth)" + checkRS.ReturnMsg);
                }
                //若有更新Token需回傳
                if (checkRS.TokenChgFlag)
                {
                    retResp.TokenChgFlag = checkRS.TokenChgFlag;
                    retResp.UserSecurityInfo = checkRS.UserSecurityInfo;
                }

                // 匯出 Report
                var path = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Temp/");
                var baseRsp = _svc.ExportCodingBook(request, ref retResp, path);
                if (baseRsp.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, ErrorCode.ProcessError, baseRsp.ReturnMsg);
                }

                retResp.ExcelUrl = ServerPathUtility.GetVirtualPath(Request, "/Content/Temp/" + retResp.FileName);

                return Result.NormalResult(request, retResp);
            }
            else
            {
                string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
                return Result.NormalResult(request, retResp, ErrorCode.ArgInvalid, errorMessage);
            }
        }

        /// <summary>
        /// 匯出資料
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [IgnoreResponseContent]
        [HttpGet]
        public IHttpActionResult ExportData(int ID)
        {
            var request = new ExportDataM.ExportDataReq()
            {
                ID = ID
            };
            var response = new ExportDataM.ExportDataRsp();
            var path = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/Temp");
            var baseRsp = _svc.ExportData(request, ref response, path);
            if (baseRsp.ReturnCode != ErrorCode.OK)
            {
                return NotFound();
            }

            var cdhv = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            cdhv.FileName = response.FileName;
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new ByteArrayContent(File.ReadAllBytes(response.FilePath));
            result.Content.Headers.ContentDisposition = cdhv;
            result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/vnd.ms-excel");

            return ResponseMessage(result);
        }

        /// <summary>
        /// 新增 Report 權限
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<AddReportAuthM.AddReportAuthRsp> AddReportAuth(AddReportAuthM.AddReportAuthReq request)
        {
            var retResp = new AddReportAuthM.AddReportAuthRsp();

            if (ModelState.IsValid)
            {
                //檢核權限
                var checkRS = await _authHandler.CheckFuncAuth(request);
                if (checkRS.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, checkRS.ReturnCode, "(Auth)" + checkRS.ReturnMsg);
                }
                //若有更新Token需回傳
                if (checkRS.TokenChgFlag)
                {
                    retResp.TokenChgFlag = checkRS.TokenChgFlag;
                    retResp.UserSecurityInfo = checkRS.UserSecurityInfo;
                }

                // 新增 Report 權限
                var serviceResponse = _svc.AddReportAuth(request, ref retResp);
                if (serviceResponse.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, ErrorCode.ProcessError, serviceResponse.ReturnMsg);
                }

                return Result.NormalResult(request, retResp);
            }
            else
            {
                string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
                return Result.NormalResult(request, retResp, ErrorCode.ArgInvalid, errorMessage);
            }
        }


        /// <summary>
        /// 匯入資料
        /// </summary>
        /// <returns></returns>
        [IgnoreRequestContent]
        public async Task<ImportDataM.ImportDataRsp> ImportData()
        {
            var request = new ImportDataM.ImportDataReq();
            var retResp = new ImportDataM.ImportDataRsp();

            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = HttpContext.Current.Server.MapPath("~/App_Data/Temp");
            Directory.CreateDirectory(root);
            var provider = new MultipartFormDataStreamProvider(root);

            await Request.Content.ReadAsMultipartAsync(provider);

            try
            {
                request.AccID = provider.FormData["AccID"];
                request.FuncCode = provider.FormData["FuncCode"];
                request.AuthCode = int.Parse(provider.FormData["AuthCode"]);
                request.UserSecurityInfo = provider.FormData["UserSecurityInfo"];
                request.QuestIDRowNum = int.Parse(provider.FormData["QuestIDRowNum"]);
                request.DataStartRowNum = int.Parse(provider.FormData["DataStartRowNum"]);
                request.ExcelPath = provider.FileData[0].LocalFileName.Replace("\"", "");
                request.ForceInsert = bool.Parse(provider.FormData["ForceInsert"]);
            }
            catch (Exception e)
            {
                return Result.NormalResult(request, retResp, ErrorCode.ArgInvalid, "匯入參數格式錯誤");
            }

            //檢核權限
            var checkRS = await _authHandler.CheckFuncAuth(request);
            if (checkRS.ReturnCode != ErrorCode.OK)
            {
                return Result.NormalResult(request, retResp, checkRS.ReturnCode, "(Auth)" + checkRS.ReturnMsg);
            }
            //若有更新Token需回傳
            if (checkRS.TokenChgFlag)
            {
                retResp.TokenChgFlag = checkRS.TokenChgFlag;
                retResp.UserSecurityInfo = checkRS.UserSecurityInfo;
            }

            // 新增問題檔案
            var serviceRsp = _svc.ImportData(request, ref retResp);
            if (serviceRsp.ReturnCode != ErrorCode.OK)
            {
                return Result.NormalResult(request, retResp, ErrorCode.ProcessError, serviceRsp.ReturnMsg);
            }

            return Result.NormalResult(request, retResp);
        }

        /// <summary>
        /// 取得報告模板清單
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<GetReportTemplateListM.GetReportTemplateListRsp> GetReportTemplateList(GetReportTemplateListM.GetReportTemplateListReq request)
        {
            var retResp = new GetReportTemplateListM.GetReportTemplateListRsp();

            if (ModelState.IsValid)
            {
                //檢核權限
                var checkRS = await _authHandler.CheckFuncAuth(request);
                if (checkRS.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, checkRS.ReturnCode, "(Auth)" + checkRS.ReturnMsg);
                }
                //若有更新Token需回傳
                if (checkRS.TokenChgFlag)
                {
                    retResp.TokenChgFlag = checkRS.TokenChgFlag;
                    retResp.UserSecurityInfo = checkRS.UserSecurityInfo;
                }

                // 新增 Report 權限
                var serviceResponse = _svc.GetReportTemplateList(request, ref retResp);
                if (serviceResponse.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, ErrorCode.ProcessError, serviceResponse.ReturnMsg);
                }

                return Result.NormalResult(request, retResp);
            }
            else
            {
                string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
                return Result.NormalResult(request, retResp, ErrorCode.ArgInvalid, errorMessage);
            }
        }

        /// <summary>
        /// 取得匯出報告模板額外問題清單
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<GetETemplateEQuestListM.Response> GetETemplateEQuestList(GetETemplateEQuestListM.Request request)
        {
            var retResp = new GetETemplateEQuestListM.Response();

            if (ModelState.IsValid)
            {
                //檢核權限
                var checkRS = await _authHandler.CheckFuncAuth(request);
                if (checkRS.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, checkRS.ReturnCode, "(Auth)" + checkRS.ReturnMsg);
                }
                //若有更新Token需回傳
                if (checkRS.TokenChgFlag)
                {
                    retResp.TokenChgFlag = checkRS.TokenChgFlag;
                    retResp.UserSecurityInfo = checkRS.UserSecurityInfo;
                }
                
                var serviceResponse = _svc.GetETemplateEQuestList(request, ref retResp);
                if (serviceResponse.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, ErrorCode.ProcessError, serviceResponse.ReturnMsg);
                }

                return Result.NormalResult(request, retResp);
            }
            else
            {
                string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
                return Result.NormalResult(request, retResp, ErrorCode.ArgInvalid, errorMessage);
            }
        }

        /// <summary>
        /// 取得報告模板資料
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<GetReportTemplateM.Response> GetReportTemplate(GetReportTemplateM.Request request)
        {
            var retResp = new GetReportTemplateM.Response();

            if (ModelState.IsValid)
            {
                //檢核權限
                X1TokenAuthCheckM.X1TokenAuthCheckReq x1TokenAuthCheckReq = new X1TokenAuthCheckM.X1TokenAuthCheckReq()
                {
                    AccID = request.AccID,
                    FuncCode = request.FuncCode,
                    AuthCode = request.AuthCode,
                    UserSecurityInfo = request.UserSecurityInfo
                };

                var checkRS = await _authHandler.CheckFuncAuth(x1TokenAuthCheckReq);
                if (checkRS.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, checkRS.ReturnCode, "(Auth)" + checkRS.ReturnMsg);
                }
                //若有更新Token需回傳
                if (checkRS.TokenChgFlag)
                {
                    retResp.TokenChgFlag = checkRS.TokenChgFlag;
                    retResp.UserSecurityInfo = checkRS.UserSecurityInfo;
                }

                string rootPath = System.Web.Hosting.HostingEnvironment.MapPath("~");
                var ExportReportRS = _svc.GetReportTemplate(request, ref retResp, rootPath);
                if (ExportReportRS.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, ErrorCode.ProcessError, ExportReportRS.ReturnMsg);
                }

                retResp.Data.FileUrl = ServerPathUtility.GetVirtualPath(Request, "/Content/Temp/" + retResp.Data.FileUrl);

                return Result.NormalResult(request, retResp);
            }
            else
            {
                string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
                return Result.NormalResult(request, retResp, ErrorCode.ArgInvalid, errorMessage);
            }
        }

        /// <summary>
        /// 匯出子宮頸國建署資料
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ExportCervixDataM.Response> ExportCervixData(ExportCervixDataM.Request request)
        {

            var retResp = new ExportCervixDataM.Response();

            if (ModelState.IsValid)
            {
                //檢核權限
                X1TokenAuthCheckM.X1TokenAuthCheckReq x1TokenAuthCheckReq = new X1TokenAuthCheckM.X1TokenAuthCheckReq()
                {
                    AccID = request.AccID,
                    FuncCode = request.FuncCode,
                    AuthCode = request.AuthCode,
                    UserSecurityInfo = request.UserSecurityInfo
                };

                var checkRS = await _authHandler.CheckFuncAuth(x1TokenAuthCheckReq);
                if (checkRS.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, checkRS.ReturnCode, "(Auth)" + checkRS.ReturnMsg);
                }
                //若有更新Token需回傳
                if (checkRS.TokenChgFlag)
                {
                    retResp.TokenChgFlag = checkRS.TokenChgFlag;
                    retResp.UserSecurityInfo = checkRS.UserSecurityInfo;
                }

                // 匯出 Report  TingYU
                var sessionkey = HttpContext.Current.Request.Headers["SessionKey"];
                var DMSSharesetting = new DMSShareService(_uow, _idoctorSvc);
                var WebDB = DMSSharesetting.GetDMSSetting(sessionkey).Web_db;


                string rootPath = System.Web.Hosting.HostingEnvironment.MapPath("~");
                var ExportReportRS = _svc.ExportCervixData(request, ref retResp, rootPath, WebDB);
                if (ExportReportRS.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, ErrorCode.ProcessError, ExportReportRS.ReturnMsg);
                }

                retResp.ExcelUrl = ServerPathUtility.GetVirtualPath(Request, "/Content/Temp/" + retResp.ExcelUrl);

                return Result.NormalResult(request, retResp);
            }
            else
            {
                string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
                return Result.NormalResult(request, retResp, ErrorCode.ArgInvalid, errorMessage);
            }
        }

        /// <summary>
        /// 匯出Excel
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ExportExcelM.Response> ExportExcel(ExportExcelM.Request request)
        {

            var retResp = new ExportExcelM.Response();

            if (ModelState.IsValid)
            {
                //檢核權限
                X1TokenAuthCheckM.X1TokenAuthCheckReq x1TokenAuthCheckReq = new X1TokenAuthCheckM.X1TokenAuthCheckReq()
                {
                    AccID = request.AccID,
                    FuncCode = request.FuncCode,
                    AuthCode = request.AuthCode,
                    UserSecurityInfo = request.UserSecurityInfo
                };

                var checkRS = await _authHandler.CheckFuncAuth(x1TokenAuthCheckReq);
                if (checkRS.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, checkRS.ReturnCode, "(Auth)" + checkRS.ReturnMsg);
                }
                //若有更新Token需回傳
                if (checkRS.TokenChgFlag)
                {
                    retResp.TokenChgFlag = checkRS.TokenChgFlag;
                    retResp.UserSecurityInfo = checkRS.UserSecurityInfo;
                }

                // 匯出 Report
                string rootPath = System.Web.Hosting.HostingEnvironment.MapPath("~");
                var ExportReportRS = _svc.ExportExcel(request, ref retResp, rootPath);
                if (ExportReportRS.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, ErrorCode.ProcessError, ExportReportRS.ReturnMsg);
                }

                retResp.ExcelUrl = ServerPathUtility.GetVirtualPath(Request, "/Content/Temp/" + retResp.ExcelUrl);

                return Result.NormalResult(request, retResp);
            }
            else
            {
                string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
                return Result.NormalResult(request, retResp, ErrorCode.ArgInvalid, errorMessage);
            }
        }


        /// <summary>
        /// 取得問題資料
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<CheckQuestNo.Response> CheckQuestNo(CheckQuestNo.Request request)
        {
            var retResp = new CheckQuestNo.Response();

            if (ModelState.IsValid)
            {
                //檢核權限
                X1TokenAuthCheckM.X1TokenAuthCheckReq x1TokenAuthCheckReq = new X1TokenAuthCheckM.X1TokenAuthCheckReq()
                {
                    AccID = request.AccID,
                    FuncCode = request.FuncCode,
                    AuthCode = request.AuthCode,
                    UserSecurityInfo = request.UserSecurityInfo
                };

                var checkRS = await _authHandler.CheckFuncAuth(x1TokenAuthCheckReq);
                if (checkRS.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, checkRS.ReturnCode, "(Auth)" + checkRS.ReturnMsg);
                }
                //若有更新Token需回傳
                if (checkRS.TokenChgFlag)
                {
                    retResp.TokenChgFlag = checkRS.TokenChgFlag;
                    retResp.UserSecurityInfo = checkRS.UserSecurityInfo;
                }

                string rootPath = System.Web.Hosting.HostingEnvironment.MapPath("~");
                var ExportReportRS = _svc.CheckQuestNo(request, ref retResp);
                if (ExportReportRS.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, ErrorCode.ProcessError, ExportReportRS.ReturnMsg);
                }

                //retResp.Data.FileUrl = Request.GetRequestContext().VirtualPathRoot + "/Content/Temp/" + retResp.Data.FileUrl;

                return Result.NormalResult(request, retResp);
            }
            else
            {
                string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
                return Result.NormalResult(request, retResp, ErrorCode.ArgInvalid, errorMessage);
            }
        }


        /// <summary>
        /// 取得關注問題清單
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<GetPinQuestM.Response> GetPinQuest(GetPinQuestM.Request request)
        {
            var retResp = new GetPinQuestM.Response();

            if (ModelState.IsValid)
            {
                //檢核權限
                X1TokenAuthCheckM.X1TokenAuthCheckReq x1TokenAuthCheckReq = new X1TokenAuthCheckM.X1TokenAuthCheckReq()
                {
                    AccID = request.AccID,
                    FuncCode = request.FuncCode,
                    AuthCode = request.AuthCode,
                    UserSecurityInfo = request.UserSecurityInfo
                };

                var checkRS = await _authHandler.CheckFuncAuth(x1TokenAuthCheckReq);
                if (checkRS.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, checkRS.ReturnCode, "(Auth)" + checkRS.ReturnMsg);
                }
                //若有更新Token需回傳
                if (checkRS.TokenChgFlag)
                {
                    retResp.TokenChgFlag = checkRS.TokenChgFlag;
                    retResp.UserSecurityInfo = checkRS.UserSecurityInfo;
                }

                string rootPath = System.Web.Hosting.HostingEnvironment.MapPath("~");
                var ExportReportRS = _svc.GetPinQuestData(request, ref retResp);
                if (ExportReportRS.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, ErrorCode.ProcessError, ExportReportRS.ReturnMsg);
                }

                //retResp.Data.FileUrl = Request.GetRequestContext().VirtualPathRoot + "/Content/Temp/" + retResp.Data.FileUrl;

                return Result.NormalResult(request, retResp);
            }
            else
            {
                string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
                return Result.NormalResult(request, retResp, ErrorCode.ArgInvalid, errorMessage);
            }
        }


        /// <summary>
        /// 更新關注問題顯示及排序
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<UpdatePinnedQuestM.Response> UpdatePinnedQuest(UpdatePinnedQuestM.Request request)
        {
            var retResp = new UpdatePinnedQuestM.Response();

            if (ModelState.IsValid)
            {
                //檢核權限
                X1TokenAuthCheckM.X1TokenAuthCheckReq x1TokenAuthCheckReq = new X1TokenAuthCheckM.X1TokenAuthCheckReq()
                {
                    AccID = request.AccID,
                    FuncCode = request.FuncCode,
                    AuthCode = request.AuthCode,
                    UserSecurityInfo = request.UserSecurityInfo
                };

                var checkRS = await _authHandler.CheckFuncAuth(x1TokenAuthCheckReq);
                if (checkRS.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, checkRS.ReturnCode, "(Auth)" + checkRS.ReturnMsg);
                }
                //若有更新Token需回傳
                if (checkRS.TokenChgFlag)
                {
                    retResp.TokenChgFlag = checkRS.TokenChgFlag;
                    retResp.UserSecurityInfo = checkRS.UserSecurityInfo;
                }

                string rootPath = System.Web.Hosting.HostingEnvironment.MapPath("~");
                var ExportReportRS = _svc.UpdatePinnedQuest(request, ref retResp);
                if (ExportReportRS.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, ErrorCode.ProcessError, ExportReportRS.ReturnMsg);
                }

                //retResp.Data.FileUrl = Request.GetRequestContext().VirtualPathRoot + "/Content/Temp/" + retResp.Data.FileUrl;

                return Result.NormalResult(request, retResp);
            }
            else
            {
                string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
                return Result.NormalResult(request, retResp, ErrorCode.ArgInvalid, errorMessage);
            }
        }


        /// <summary>
        /// 取得所有關注問題
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<GetPinnedQuestListM.Response> GetPinnedQuestList(GetPinnedQuestListM.Request request)
        {
            var retResp = new GetPinnedQuestListM.Response();

            if (ModelState.IsValid)
            {
                //檢核權限
                X1TokenAuthCheckM.X1TokenAuthCheckReq x1TokenAuthCheckReq = new X1TokenAuthCheckM.X1TokenAuthCheckReq()
                {
                    AccID = request.AccID,
                    FuncCode = request.FuncCode,
                    AuthCode = request.AuthCode,
                    UserSecurityInfo = request.UserSecurityInfo
                };

                var checkRS = await _authHandler.CheckFuncAuth(x1TokenAuthCheckReq);
                if (checkRS.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, checkRS.ReturnCode, "(Auth)" + checkRS.ReturnMsg);
                }
                //若有更新Token需回傳
                if (checkRS.TokenChgFlag)
                {
                    retResp.TokenChgFlag = checkRS.TokenChgFlag;
                    retResp.UserSecurityInfo = checkRS.UserSecurityInfo;
                }

                string rootPath = System.Web.Hosting.HostingEnvironment.MapPath("~");
                var ExportReportRS = _svc.GetPinnedQuestList(request, ref retResp);
                if (ExportReportRS.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, ErrorCode.ProcessError, ExportReportRS.ReturnMsg);
                }

                //retResp.Data.FileUrl = Request.GetRequestContext().VirtualPathRoot + "/Content/Temp/" + retResp.Data.FileUrl;

                return Result.NormalResult(request, retResp);
            }
            else
            {
                string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
                return Result.NormalResult(request, retResp, ErrorCode.ArgInvalid, errorMessage);
            }
        }


        /// <summary>
        /// 更新病患個人關注問題
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<UpdatePersonalPinnedQuestM.Response> UpdatePersonalPinnedQuest(UpdatePersonalPinnedQuestM.Request request)
        {
            var retResp = new UpdatePersonalPinnedQuestM.Response();

            if (ModelState.IsValid)
            {
                //檢核權限
                X1TokenAuthCheckM.X1TokenAuthCheckReq x1TokenAuthCheckReq = new X1TokenAuthCheckM.X1TokenAuthCheckReq()
                {
                    AccID = request.AccID,
                    FuncCode = request.FuncCode,
                    AuthCode = request.AuthCode,
                    UserSecurityInfo = request.UserSecurityInfo
                };

                var checkRS = await _authHandler.CheckFuncAuth(x1TokenAuthCheckReq);
                if (checkRS.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, checkRS.ReturnCode, "(Auth)" + checkRS.ReturnMsg);
                }
                //若有更新Token需回傳
                if (checkRS.TokenChgFlag)
                {
                    retResp.TokenChgFlag = checkRS.TokenChgFlag;
                    retResp.UserSecurityInfo = checkRS.UserSecurityInfo;
                }

                string rootPath = System.Web.Hosting.HostingEnvironment.MapPath("~");
                var ExportReportRS = _svc.UpdatePersonalPinnedQuest(request, ref retResp);
                if (ExportReportRS.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, ErrorCode.ProcessError, ExportReportRS.ReturnMsg);
                }

                //retResp.Data.FileUrl = Request.GetRequestContext().VirtualPathRoot + "/Content/Temp/" + retResp.Data.FileUrl;

                return Result.NormalResult(request, retResp);
            }
            else
            {
                string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
                return Result.NormalResult(request, retResp, ErrorCode.ArgInvalid, errorMessage);
            }
        }


        /// <summary>
        /// 取得所有病患個人關注問題
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<GetPersonalPinnedQuestListM.Response> GetPersonalPinnedQuestList(GetPersonalPinnedQuestListM.Request request)
        {
            var retResp = new GetPersonalPinnedQuestListM.Response();

            if (ModelState.IsValid)
            {
                //檢核權限
                X1TokenAuthCheckM.X1TokenAuthCheckReq x1TokenAuthCheckReq = new X1TokenAuthCheckM.X1TokenAuthCheckReq()
                {
                    AccID = request.AccID,
                    FuncCode = request.FuncCode,
                    AuthCode = request.AuthCode,
                    UserSecurityInfo = request.UserSecurityInfo
                };

                var checkRS = await _authHandler.CheckFuncAuth(x1TokenAuthCheckReq);
                if (checkRS.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, checkRS.ReturnCode, "(Auth)" + checkRS.ReturnMsg);
                }
                //若有更新Token需回傳
                if (checkRS.TokenChgFlag)
                {
                    retResp.TokenChgFlag = checkRS.TokenChgFlag;
                    retResp.UserSecurityInfo = checkRS.UserSecurityInfo;
                }

                string rootPath = System.Web.Hosting.HostingEnvironment.MapPath("~");
                var ExportReportRS = _svc.GetPersonalPinnedQuestList(request, ref retResp);
                if (ExportReportRS.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, ErrorCode.ProcessError, ExportReportRS.ReturnMsg);
                }

                //retResp.Data.FileUrl = Request.GetRequestContext().VirtualPathRoot + "/Content/Temp/" + retResp.Data.FileUrl;

                return Result.NormalResult(request, retResp);
            }
            else
            {
                string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
                return Result.NormalResult(request, retResp, ErrorCode.ArgInvalid, errorMessage);
            }
        }


        /// <summary>
        /// 刪除所有病患個人關注項目
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<DeletePersonalPinnedQuestM.Response> DeletePersonalPinnedQuest(DeletePersonalPinnedQuestM.Request request)
        {
            var retResp = new DeletePersonalPinnedQuestM.Response();

            if (ModelState.IsValid)
            {
                //檢核權限
                X1TokenAuthCheckM.X1TokenAuthCheckReq x1TokenAuthCheckReq = new X1TokenAuthCheckM.X1TokenAuthCheckReq()
                {
                    AccID = request.AccID,
                    FuncCode = request.FuncCode,
                    AuthCode = request.AuthCode,
                    UserSecurityInfo = request.UserSecurityInfo
                };

                var checkRS = await _authHandler.CheckFuncAuth(x1TokenAuthCheckReq);
                if (checkRS.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, checkRS.ReturnCode, "(Auth)" + checkRS.ReturnMsg);
                }
                //若有更新Token需回傳
                if (checkRS.TokenChgFlag)
                {
                    retResp.TokenChgFlag = checkRS.TokenChgFlag;
                    retResp.UserSecurityInfo = checkRS.UserSecurityInfo;
                }

                string rootPath = System.Web.Hosting.HostingEnvironment.MapPath("~");
                var ExportReportRS = _svc.DeletePersonalPinnedQuest(request, ref retResp);
                if (ExportReportRS.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, ErrorCode.ProcessError, ExportReportRS.ReturnMsg);
                }

                //retResp.Data.FileUrl = Request.GetRequestContext().VirtualPathRoot + "/Content/Temp/" + retResp.Data.FileUrl;

                return Result.NormalResult(request, retResp);
            }
            else
            {
                string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
                return Result.NormalResult(request, retResp, ErrorCode.ArgInvalid, errorMessage);
            }
        }
    }
}
