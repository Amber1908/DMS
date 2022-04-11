using BMDC.Models.Auth;
using BMDC.Models.Log;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebApplication1.Infrastructure.Common;
using WebApplication1.Infrastructure.Filters;
using WebApplication1.Misc;
using X1APServer.Service;
using X1APServer.Service.Interface;
using X1APServer.Service.Model;

namespace WebApplication1.WebApi
{
    public class PatientController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private IPatientService _svc = null;
        private AuthHandler _authHandler;

        public PatientController(IPatientService svc)
        {
            _svc = svc;
            _authHandler = new AuthHandler(AuthHandler.System.X1Web);
        }

        /// <summary>
        /// 新增個案
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<AddPatientInfoM.AddPatientInfoRsp> AddPatientInfo(AddPatientInfoM.AddPatientInfoReq request)
        {

            AddPatientInfoM.AddPatientInfoRsp retResp = new AddPatientInfoM.AddPatientInfoRsp();

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

                //查詢待處理清單
                var GetInfoRS = _svc.AddPatientInfo(request, ref retResp);
                if (GetInfoRS.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, ErrorCode.ProcessError, GetInfoRS.ReturnMsg);
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
        /// 取得單一個案
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<GetPatientInfoM.GetPatientInfoRsp> GetPatientInfo(GetPatientInfoM.GetPatientInfoReq request)
        {

            GetPatientInfoM.GetPatientInfoRsp retResp = new GetPatientInfoM.GetPatientInfoRsp();

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

                //查詢待處理清單
                var AddPatientInfo = _svc.GetPatientInfo(request, ref retResp);
                if (AddPatientInfo.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, ErrorCode.ProcessError, AddPatientInfo.ReturnMsg);
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
        /// 延遲取得個案清單列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<GetPatientsLazyM.GetPatientLazyRsp> GetPatientsLazy(GetPatientsLazyM.GetPatientLazyReq request)
        {

            GetPatientsLazyM.GetPatientLazyRsp retResp = new GetPatientsLazyM.GetPatientLazyRsp();

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

                //查詢待處理清單
                var GetPatientsRS = _svc.GetPatientsLazy(request, ref retResp);
                if (GetPatientsRS.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, ErrorCode.ProcessError, GetPatientsRS.ReturnMsg);
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
        /// 更新病患群組
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<UpdateGroupM.UpdateGroupRsp> UpdateGroup(UpdateGroupM.UpdateGroupReq request)
        {

            var retResp = new UpdateGroupM.UpdateGroupRsp();

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

                // 更新病患群組
                var GetPatientsRS = _svc.UpdateGroup(request, ref retResp);
                if (GetPatientsRS.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, ErrorCode.ProcessError, GetPatientsRS.ReturnMsg);
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
        /// 取得所有群組
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<GetAllGroupM.GetAllGroupRsp> GetAllGroup(GetAllGroupM.GetAllGroupReq request)
        {
            var retResp = new GetAllGroupM.GetAllGroupRsp();

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

                // 更新病患群組
                var GetPatientsRS = _svc.GetAllGroup(request, ref retResp);
                if (GetPatientsRS.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, ErrorCode.ProcessError, GetPatientsRS.ReturnMsg);
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
        /// 新增或更新回診時間
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<AddOrUpdateScheduleM.Response> AddOrUpdateSchedule(AddOrUpdateScheduleM.Request request)
        {
            var retResp = new AddOrUpdateScheduleM.Response();

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

                var GetPatientsRS = _svc.AddOrUpdateSchedule(request, ref retResp);
                if (GetPatientsRS.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, ErrorCode.ProcessError, GetPatientsRS.ReturnMsg);
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
        /// 取回診時間清單
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<GetScheduleListM.Response> GetScheduleList(GetScheduleListM.Request request)
        {
            var retResp = new GetScheduleListM.Response();

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

                var GetPatientsRS = _svc.GetScheduleList(request, ref retResp);
                if (GetPatientsRS.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, ErrorCode.ProcessError, GetPatientsRS.ReturnMsg);
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
        /// 刪除回診時間
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<DeleteScheduleM.Response> DeleteSchedule(DeleteScheduleM.Request request)
        {
            var retResp = new DeleteScheduleM.Response();

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

                var GetPatientsRS = _svc.DeleteSchedule(request, ref retResp);
                if (GetPatientsRS.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, ErrorCode.ProcessError, GetPatientsRS.ReturnMsg);
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
        /// 新增個案、診斷、檢體
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<AddPatientM.AddPatientRsp> AddPatient(AddPatientM.AddPatientReq request)
        {

            AddPatientM.AddPatientRsp retResp = new AddPatientM.AddPatientRsp();

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
                // 若有更新Token需回傳
                if (checkRS.TokenChgFlag)
                {
                    retResp.TokenChgFlag = checkRS.TokenChgFlag;
                    retResp.UserSecurityInfo = checkRS.UserSecurityInfo;
                }

                // 新增個案
                var AddPatientRS = _svc.AddPatient(request, ref retResp);
                if (AddPatientRS.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, ErrorCode.ProcessError, AddPatientRS.ReturnMsg);
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
