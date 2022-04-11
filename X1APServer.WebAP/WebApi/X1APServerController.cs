using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using iDoctorTools.Models;
using NLog;
using WebApplication1.Infrastructure.Common;
using WebApplication1.Misc;
using X1APServer.Repository;
using X1APServer.Service.Interface;
using X1APServer.Service.Model;
using X1APServer.Service.Service.Interface;
using static X1APServer.Service.Model.GetAreaCodeLazyM;

namespace WebApplication1.WebApi
{
    /// <summary>
    /// X1 Imaging 系統專用 API.
    /// </summary>
    [Authorize(Users = "X1Imaging")]
    [ApiExplorerSettings(IgnoreApi = false)]
    public class X1APServerController : ApiController
    {
        private IIDoctorService _idoctorSvc;
        private IX1APService _svc;

        private Logger logger = LogManager.GetCurrentClassLogger();

        public X1APServerController(IIDoctorService idoctorSvc, IX1APService svc)
        {
            _idoctorSvc = idoctorSvc;
            _svc = svc;
        }

        /// <summary>
        /// 取得醫生資訊
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<X1UserDataGetM.X1UserDataGetRsp> GetUserList(X1UserDataGetM.X1UserDataGetReq request)
        {
            X1UserDataGetM.X1UserDataGetRsp retResp = new X1UserDataGetM.X1UserDataGetRsp();
            if (ModelState.IsValid)
            {
                //查詢待處理清單
                retResp = _svc.GetUserList(request);
                if (retResp.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, ErrorCode.ProcessError, retResp.ReturnMsg);
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
        /// 取得檢體清單
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<GetCervixFormM.GetCervixFormRsp> GetCervixForm(GetCervixFormM.GetCervixFormReq request)
        {
            GetCervixFormM.GetCervixFormRsp retResp = new GetCervixFormM.GetCervixFormRsp();
            if (ModelState.IsValid)
            {
                //查詢待處理清單
                retResp = _svc.GetCervixForm(request);
                if (retResp.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, ErrorCode.ProcessError, retResp.ReturnMsg);
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
        /// 取得檢驗單明細
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<GetCervixTableM.GetCervixTableRsp> GetCervixTable(GetCervixTableM.GetCervixTableReq request)
        {
            GetCervixTableM.GetCervixTableRsp retResp = new GetCervixTableM.GetCervixTableRsp();
            if (ModelState.IsValid)
            {
                //查詢待處理清單
                retResp = _svc.GetCervixTable(request);
                if (retResp.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, ErrorCode.ProcessError, retResp.ReturnMsg);
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
        public async Task<UpdateCervixStatusM.UpdateCervixStatusRsp> UpdateCervixStatus(UpdateCervixStatusM.UpdateCervixStatusReq request)
        {
            UpdateCervixStatusM.UpdateCervixStatusRsp retResp = new UpdateCervixStatusM.UpdateCervixStatusRsp();
            if (ModelState.IsValid)
            {
                //查詢待處理清單
                retResp = _svc.UpdateCervixStatus(request);
                if (retResp.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, ErrorCode.ProcessError, retResp.ReturnMsg);
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
        /// 更新檢驗單詳細資料
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<UpdateCervixTableM.UpdateCervixTableRsp> UpdateCervixTable(UpdateCervixTableM.UpdateCervixTableReq request)
        {
            UpdateCervixTableM.UpdateCervixTableRsp retResp = new UpdateCervixTableM.UpdateCervixTableRsp();
            if (ModelState.IsValid)
            {
                //查詢待處理清單
                retResp = _svc.UpdateCervixTable(request);
                if (retResp.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, ErrorCode.ProcessError, retResp.ReturnMsg);
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
        /// 取得X1儲存資料
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<GetX1DataM.Response> GetX1Data(GetX1DataM.Request request)
        {
            GetX1DataM.Response retResp = new GetX1DataM.Response();
            if (ModelState.IsValid)
            {
                //查詢待處理清單
                retResp = _svc.GetX1Data(request);
                if (retResp.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, ErrorCode.ProcessError, retResp.ReturnMsg);
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
        /// X1儲存資料
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<UpdateX1DataM.UpdateX1DataRsp> UpdateX1Data(UpdateX1DataM.UpdateX1DataReq request)
        {
            UpdateX1DataM.UpdateX1DataRsp retResp = new UpdateX1DataM.UpdateX1DataRsp();
            if (ModelState.IsValid)
            {
                //查詢待處理清單
                retResp = _svc.UpdateX1Data(request);
                if (retResp.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, ErrorCode.ProcessError, retResp.ReturnMsg);
                }

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