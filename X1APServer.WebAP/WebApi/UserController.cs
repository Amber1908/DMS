using System;
using System.Collections.Generic;
using System.Web.Http;
using NLog;
using Newtonsoft.Json;
using X1APServer.Service.Interface;
using X1APServer.Service.Model;
using BMDC.Models.Auth;
using BMDC.Models.Log;
using BMDC.Models;
using System.Configuration;
using System.Net.Http;
using System.Web;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http.Headers;
using System.Linq;
using WebApplication1.Misc;
using WebApplication1.Infrastructure;
using WebApplication1.Infrastructure.Common;
using WebApplication1.Infrastructure.Filters;
using X1APServer.Service.AuthService;
using iDoctorTools.Models;
using WebApplication1.Infrastructure.Utility;
using WebApplication1.Infrastructure.Common.Interface;
using static X1APServer.Service.Model.GetAreaCodeLazyM;
using System.Text.RegularExpressions;
using X1APServer.Service.Service;


namespace WebApplication1.WebApi
{
    public class UserController : ApiController
    {
        private static AuthCommon.ServiceInfo _serviceInfo = null;
        private AuthHandler _authHandler = null;
        private static readonly string _SysCode = ConfigurationManager.AppSettings["WebSysCode"];
        private static readonly string _FrameServiceToken = ConfigurationManager.AppSettings["WebFrameServiceToken"];
        private IIDoctorService _idoctorSvc;
        private IDMSShareService _dmsShareSvc;
        private IFrameRequest _frameReq;
        //private IReportService _reportService;
        private Logger logger = LogManager.GetCurrentClassLogger();

        public UserController(IIDoctorService idoctorSvc, IDMSShareService dmsShareSvc, IFrameRequest frameReq)
        {
            
            _serviceInfo = new AuthCommon.ServiceInfo()
            {
                SysCode = _SysCode,
                ServiceKey = _FrameServiceToken
            };

            _authHandler = new AuthHandler(AuthHandler.System.X1Web);
            _idoctorSvc = idoctorSvc;
            _dmsShareSvc = dmsShareSvc;
            _frameReq = frameReq;
        }

        /// <summary>
        /// Web 登入
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<X1LoginCheckM.X1LoginCheckRsp> Login(X1LoginCheckM.X1LoginCheckReq request)
        {
            X1LoginCheckM.X1LoginCheckRsp retResp = new X1LoginCheckM.X1LoginCheckRsp();
            //try
            //{
            if (ModelState.IsValid)
            {
                var response = await _idoctorSvc.UserLogin(request.AccID, request.AccPWD);
                
                switch (response.statuscode)
                {
                    case "0000":
                        var token = _dmsShareSvc.AddUserToken(request.AccID);
                        retResp.Token = token.Token;
                        retResp.AccID = request.AccID;
                        break;
                    case "0001":
                    case "0002":
                    case "1003":
                    case "2001":
                        return Result.NormalResult(request, retResp, ErrorCode.ProcessError, response.statusdesc);
                    case "1001":
                    case "1002":
                        return Result.NormalResult(request, retResp, ErrorCode.AccountInvalid, "帳號密碼錯誤");
                    case "3001":
                        return Result.NormalResult(request, retResp, ErrorCode.EmailNotVerify, "Email 尚未驗證");
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
        /// 註冊使用者
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public WebRegisterM.WebRegisterRsp RegisterUser(WebRegisterM.WebRegisterReq request)
        {

            WebRegisterM.WebRegisterRsp retResp = new WebRegisterM.WebRegisterRsp();

            if (ModelState.IsValid)
            {
                var authService = new AuthServiceClient();
                RegisterM.RegisterReq registerReq = new RegisterM.RegisterReq()
                {
                    AccID = request.RequestAccID,
                    AccName = request.AccName,
                    AccPWD = request.AccPWD,
                    AccTitle = request.AccTitle,
                    CellPhone = request.CellPhone,
                    Email = request.Email,
                    IsAdmin = request.IsAdmin,
                    RoleCode = request.RoleCode
                };

                var registerRsp = authService.Register(_serviceInfo, registerReq);

                authService.Close();

                if (registerRsp.ReturnCode != 1)
                {
                    return Result.NormalResult(request, retResp, (ErrorCode)((int)ErrorCode.RegisterError + registerRsp.ReturnCode), registerRsp.ReturnMsg);
                }


                retResp.AccID = registerRsp.AccID;
                retResp.UserGUID = registerRsp.UserGUID;

                return Result.NormalResult(request, retResp);
            }
            else
            {
                string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
                return Result.NormalResult(request, retResp, ErrorCode.ArgInvalid, errorMessage);
            }
        }

        /// <summary>
        /// 更新使用者資料
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<WebUpdateUserM.WebUpdateUserRsp> UpdateUser(WebUpdateUserM.WebUpdateUserReq request)
        {

            WebUpdateUserM.WebUpdateUserRsp retResp = new WebUpdateUserM.WebUpdateUserRsp();

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

                var authService = new AuthServiceClient();

                // 準備 Frame Request Data
                var frameReuqest = _frameReq.AppendDBName(new UpdateUserM.UpdateUserReq()
                {
                    AccID = request.RequestAccID,
                    AccName = request.AccName,
                    AccTitle = request.AccTitle,
                    CellPhone = request.CellPhone,
                    Email = request.Email,
                    RoleCode = request.RoleCode,
                    UpdateMan = request.AccID,
                    DoctorNo = request.DoctorNo,
                    Senior = request.Senior
                });

                // 更新使用者資料
                var updateUserRsp = authService.UpdateUser(_serviceInfo, frameReuqest);

                authService.Close();

                if (updateUserRsp.ReturnCode != 1)
                {
                    return Result.NormalResult(request, retResp, (ErrorCode)((int)ErrorCode.RegisterError + updateUserRsp.ReturnCode), updateUserRsp.ReturnMsg);
                }

                return Result.NormalResult(request, retResp);
            }
            else
            {
                string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
                return Result.NormalResult(request, retResp, (ErrorCode)(int)ErrorCode.ArgInvalid, errorMessage);
            }
        }

        /// <summary>
        /// 更新使用者密碼
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<WebUpdatePasswordM.WebUpdatePasswordRsp> UpdateUserPassword(WebUpdatePasswordM.WebUpdatePasswordReq request)
        {

            WebUpdatePasswordM.WebUpdatePasswordRsp retResp = new WebUpdatePasswordM.WebUpdatePasswordRsp();

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

                var updateUserPwdRsp = await _idoctorSvc.UserChangePassword(request.AccID, request.OldPassword, request.NewPassword);

                if (updateUserPwdRsp.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, (ErrorCode)((int)ErrorCode.RegisterError + updateUserPwdRsp.ReturnCode), updateUserPwdRsp.ReturnMsg);
                }

                return Result.NormalResult(request, retResp);
            }
            else
            {
                string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
                return Result.NormalResult(request, retResp, (ErrorCode)(int)ErrorCode.ArgInvalid, errorMessage);
            }
        }

        /// <summary>
        /// 更新使用者使用狀態
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<WebUpdateUseStateM.WebUpdateUseStateRsp> UpdateUseState(WebUpdateUseStateM.WebUpdateUseStateReq request)
        {

            WebUpdateUseStateM.WebUpdateUseStateRsp retResp = new WebUpdateUseStateM.WebUpdateUseStateRsp();

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

                var authService = new AuthServiceClient();

                var frameRequest = _frameReq.AppendDBName(new UpdateUseStateM.UpdateUseStateReq()
                {
                    AccID = request.RequestAccID,
                    UseState = request.UseState,
                    UpdateMan = request.AccID
                });

                var frameResponse = authService.UpdateUseState(_serviceInfo, frameRequest);

                authService.Close();

                if (frameResponse.ReturnCode != 1)
                {
                    return Result.NormalResult(request, retResp, (ErrorCode)((int)ErrorCode.RegisterError + frameResponse.ReturnCode), frameResponse.ReturnMsg);
                }

                return Result.NormalResult(request, retResp);
            }
            else
            {
                string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
                return Result.NormalResult(request, retResp, (ErrorCode)(int)ErrorCode.ArgInvalid, errorMessage);
            }
        }

        /// <summary>
        /// 取得單一使用者
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<WebGetUserM.WebGetUserRsp> GetUser(WebGetUserM.WebGetUserReq request)
        {
            WebGetUserM.WebGetUserRsp retResp = new WebGetUserM.WebGetUserRsp();

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

                var authService = new AuthServiceClient();

                var frameRequest = _frameReq.AppendDBName(new GetUserM.GetUserReq()
                {
                    AccID = request.RequestAccID
                });

                var frameResponse = authService.GetUser(_serviceInfo, frameRequest);

                //檢核權限
                GetSysMenuListM.GetSysMenuListReq getSysMenuListReq = _frameReq.AppendDBName(new GetSysMenuListM.GetSysMenuListReq()
                {
                    AccID = request.AccID
                });

                GetSysMenuListM.GetSysMenuListRsp retMenu = authService.GetSysMenuList(_serviceInfo, getSysMenuListReq);
                //關閉SVC連線
                authService.Close();
                if (!retMenu.ReturnCode.Equals(1))
                {
                    return Result.NormalResult(request, retResp, ErrorCode.ProcessError, retMenu.ReturnMsg);
                }

                if (retMenu.MenuInfo.Count.Equals(0))
                {
                    return Result.NormalResult(request, retResp, ErrorCode.AuthError, "您的權限不足，因此無法登入系統，請洽系統管理人員!");
                }

                authService.Close();

                if (frameResponse.ReturnCode != 1)
                {
                    return Result.NormalResult(request, retResp, (ErrorCode)((int)ErrorCode.RegisterError + frameResponse.ReturnCode), frameResponse.ReturnMsg);
                }

                var config = new AutoMapper.MapperConfiguration(cfg =>
                    cfg.CreateMap<GetUserM.GetUserRsp, WebGetUserM.WebGetUserRsp>()
                    .ForMember(dest => dest.RoleList, opt => opt.Ignore()));
                var mapper = config.CreateMapper();
                retResp = mapper.Map<WebGetUserM.WebGetUserRsp>(frameResponse);

                var roleListConfig = new AutoMapper.MapperConfiguration(cfg => cfg.CreateMap<GetRoleListM.Role, WebGetRoleListM.Role>());
                var roleListMapper = roleListConfig.CreateMapper();
                retResp.RoleList = roleListMapper.Map<List<WebGetRoleListM.Role>>(frameResponse.RoleList);

                retResp.MenuInfo = new List<X1LoginCheckM.MenuListInfo>();
                //轉換Model Mapping
                foreach (GetSysMenuListM.MenuListInfo listinfo in retMenu.MenuInfo)
                {
                    retResp.MenuInfo.Add(
                    new X1LoginCheckM.MenuListInfo()
                    {
                        GroupCode = listinfo.GroupCode,
                        GroupName = listinfo.GroupName,
                        FuncCode = listinfo.FuncCode,
                        FuncName = listinfo.FuncName,
                        FuncPath = listinfo.FuncPath,
                        AuthNo1 = listinfo.AuthNo1,
                        AuthNo2 = listinfo.AuthNo2,
                        AuthNo3 = listinfo.AuthNo3,
                        AuthNo4 = listinfo.AuthNo4,
                        AuthNo5 = listinfo.AuthNo5,
                    });
                }

                return Result.NormalResult(request, retResp);
            }
            else
            {
                string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
                return Result.NormalResult(request, retResp, (ErrorCode)(int)ErrorCode.ArgInvalid, errorMessage);
            }

        }

        /// <summary>
        /// 取得使用者清單
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<WebGetUserListM.WebGetUserListRsp> GetUserList(WebGetUserListM.WebGetUserListReq request)
        {
            //
            WebGetUserListM.WebGetUserListRsp retResp = new WebGetUserListM.WebGetUserListRsp();

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

                var authService = new AuthServiceClient();

                var getUserListReq = _frameReq.AppendDBName(new GetUserListM.GetUserListReq()
                {
                    AccID = request.RequestedAccID,
                    AccName = request.AccName,
                    CellPhone = request.CellPhone,
                    Email = request.Email,
                    RoleCodes = request.RoleCodes
                });

                var frameResponse = authService.GetUserList(_serviceInfo, getUserListReq);

                authService.Close();

                if (frameResponse.ReturnCode != 1)
                {
                    return Result.NormalResult(request, retResp, (ErrorCode)((int)ErrorCode.RegisterError + frameResponse.ReturnCode), frameResponse.ReturnMsg);
                }

                retResp.UserList = frameResponse.UserList;

                return Result.NormalResult(request, retResp);
            }
            else
            {
                string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
                return Result.NormalResult(request, retResp, (ErrorCode)(int)ErrorCode.ArgInvalid, errorMessage);
            }
        }

        /// <summary>
        /// 取得角色清單
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<WebGetRoleListM.WebGetRoleListRsp> GetRoleList(WebGetRoleListM.WebGetRoleListReq request)
        {

            WebGetRoleListM.WebGetRoleListRsp retResp = new WebGetRoleListM.WebGetRoleListRsp();

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

                var authService = new AuthServiceClient();

                var frameRequest = _frameReq.AppendDBName(new GetRoleListM.GetRoleListReq());
                var frameResponse = authService.GetRoleList(_serviceInfo, frameRequest);

                authService.Close();

                if (frameResponse.ReturnCode != 1)
                {
                    return Result.NormalResult(request, retResp, (ErrorCode)((int)ErrorCode.ProcessError + frameResponse.ReturnCode), frameResponse.ReturnMsg);
                }

                retResp.RoleList = frameResponse.Roles.Select(x => new WebGetRoleListM.Role()
                {
                    RoleCode = x.RoleCode,
                    RoleName = x.RoleName,
                    RoleTitle = x.RoleTitle
                }).ToList();

                return Result.NormalResult(request, retResp);
            }
            else
            {
                string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
                return Result.NormalResult(request, retResp, (ErrorCode)(int)ErrorCode.ArgInvalid, errorMessage);
            }
        }

        /// <summary>
        /// 刪除使用者
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<WebDeleteUserM.WebDeleteUserRsp> DeleteUser(WebDeleteUserM.WebDeleteUserReq request)
        {

            WebDeleteUserM.WebDeleteUserRsp retResp = new WebDeleteUserM.WebDeleteUserRsp();

            if (ModelState.IsValid)
            {
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

                var authService = new AuthServiceClient();

                var frameRequest = _frameReq.AppendDBName(new DeleteUserM.DeleteUserReq()
                {
                    AccID = request.RequestedAccID
                });

                var frameResponse = authService.DeleteUser(_serviceInfo, frameRequest);

                authService.Close();

                if (frameResponse.ReturnCode != 1)
                {
                    return Result.NormalResult(request, retResp, (ErrorCode)((int)ErrorCode.ProcessError + frameResponse.ReturnCode), frameResponse.ReturnMsg);
                }

                return Result.NormalResult(request, retResp);
            }
            else
            {
                string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
                return Result.NormalResult(request, retResp, (ErrorCode)(int)ErrorCode.ArgInvalid, errorMessage);
            }
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<WebLogoutM.WebLogoutRsp> Logout(WebLogoutM.WebLogoutReq request)
        {

            WebLogoutM.WebLogoutRsp retResp = new WebLogoutM.WebLogoutRsp();

            if (ModelState.IsValid)
            {
                //檢核權限
                X1TokenAuthCheckM.X1TokenAuthCheckReq x1TokenAuthCheckReq = new X1TokenAuthCheckM.X1TokenAuthCheckReq()
                {
                    AccID = request.RequestedAccID,
                    FuncCode = request.FuncCode,
                    AuthCode = request.AuthCode,
                    UserSecurityInfo = request.UserSecurityInfo
                };

                var checkRS = await _authHandler.CheckFuncAuth(x1TokenAuthCheckReq);
                if (checkRS.ReturnCode != ErrorCode.OK)
                {
                    return Result.NormalResult(request, retResp, checkRS.ReturnCode, "(Auth)" + checkRS.ReturnMsg);
                }

                var authService = new AuthServiceClient();

                var frameRequest = _frameReq.AppendDBName(new LogoutM.LogoutReq()
                {
                    AccID = request.RequestedAccID,
                    SysCode = _serviceInfo.SysCode
                });

                var frameResponse = authService.Logout(_serviceInfo, frameRequest);

                authService.Close();

                if (frameResponse.ReturnCode != 1)
                {
                    return Result.NormalResult(request, retResp, (ErrorCode)((int)ErrorCode.ProcessError + frameResponse.ReturnCode), frameResponse.ReturnMsg);
                }

                return Result.NormalResult(request, retResp);
            }
            else
            {
                string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
                return Result.NormalResult(request, retResp, (ErrorCode)(int)ErrorCode.ArgInvalid, errorMessage);
            }
        }

        /// <summary>
        /// 取得使用者擁有站台清單
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<GetHealthWebByUserM.Response> GetHealthWebByUser(string email)
        {

            GetHealthWebByUserM.Response retResp = new GetHealthWebByUserM.Response();

            if (ModelState.IsValid)
            {
                retResp = await _idoctorSvc.GetHealthWebByUser(email);
                
                return Result.NormalResult(email, retResp);
            }
            else
            {
                string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
                return Result.NormalResult(email, retResp, (ErrorCode)(int)ErrorCode.ArgInvalid, errorMessage);
            }
        }

        /// <summary>
        /// 取得地區代碼表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<GetAreaCodeM.Response> GetAreaCode()
        {
            GetAreaCodeM.Response retResp = new GetAreaCodeM.Response();

            if (ModelState.IsValid)
            {
                retResp.Data = await _idoctorSvc.GetAreaCode();

                return Result.NormalResult("", retResp);
            }
            else
            {
                string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
                return Result.NormalResult("", retResp, (ErrorCode)(int)ErrorCode.ArgInvalid, errorMessage);
            }
        }

        /// <summary>
        /// 取得地區代碼表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<GetAreaCodeLazyM.Response> GetAreaCodeLazy()
        {
            GetAreaCodeM.Response retResp = new GetAreaCodeM.Response();
            GetAreaCodeLazyM.Response retLazy = new GetAreaCodeLazyM.Response();
            retLazy.Data = new List<AREACODELAZY>();

            if (ModelState.IsValid)
            {
                retResp.Data = await _idoctorSvc.GetAreaCode();

                foreach(AREACODE aa in retResp.Data)
                {
                    AREACODELAZY az = new AREACODELAZY()
                    {
                        text = aa.AreaCode + " " + aa.AreaName,
                        value = aa.AreaCode

                    };
                    retLazy.Data.Add(az);
                }

                return Result.NormalResult("", retLazy);
            }
            else
            {
                string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
                return Result.NormalResult("", retLazy, (ErrorCode)(int)ErrorCode.ArgInvalid, errorMessage);
            }
        }

        /// <summary>
        /// 取得醫療機構代碼表
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<GetHospitalCodeM.Response> GetHospitalCode(string code)
        {

            GetHospitalCodeM.Response retResp = new GetHospitalCodeM.Response();

            if (ModelState.IsValid)
            {
                retResp.Data = await _idoctorSvc.GetHospitalCode(code);

                return Result.NormalResult(code, retResp);
            }
            else
            {
                string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
                return Result.NormalResult(code, retResp, (ErrorCode)(int)ErrorCode.ArgInvalid, errorMessage);
            }
        }

        /// <summary>
        /// 取得醫療機構代碼表字串
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<GetHospitalCodeLazyM.Response> GetHospitalCodeLazy(GetHospitalCodeLazyM.Request request)
        
        {
            GetHospitalCodeLazyM.Response retResp = new GetHospitalCodeLazyM.Response();
           
            if (ModelState.IsValid)
            {
                
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

                if(!string.IsNullOrEmpty(request.code))
                    retResp.Data = await _idoctorSvc.GetHospitalCodeLazy(request.code);
                
                //EnterByItself(request.code);
                return Result.NormalResult(request, retResp);

            }
            else
            {
                string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
                return Result.NormalResult(request, retResp, (ErrorCode)(int)ErrorCode.ArgInvalid, errorMessage);
            }
        }

        /// <summary>
        /// 取得病程站台資訊
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<GetHealthWebM.Response> GetHealthWeb(GetHealthWebM.Request request)
        {
            var retResp = new GetHealthWebM.Response();
            if (ModelState.IsValid)
            {
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

                retResp.Data = await _idoctorSvc.GetHealthWeb(request.web_sn);

                return Result.NormalResult(request, retResp);
            }
            else
            {
                string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
                return Result.NormalResult(request, retResp, ErrorCode.ArgInvalid, errorMessage);
            }
        }

        /// <summary>
        /// 取得使用者Token
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<GetTokenM.Response> GetToken(GetTokenM.Request request)
        {
            var retResp = new GetTokenM.Response();
            if (ModelState.IsValid)
            {
                var flag = _dmsShareSvc.CheckToken(request.AccID, request.LoginToken);
                if (!flag)
                {
                    return Result.NormalResult(request, retResp, ErrorCode.LoginTokenInvalid, "登入token錯誤將導回登入頁面");
                }

                var session = await _idoctorSvc.GenSession(request.AccID, request.Web_sn);
                var websetting = await _idoctorSvc.GetHealthWeb(request.Web_sn);
                var allUser = await _idoctorSvc.GetUserByHealthWeb(websetting.web_sn);

                _dmsShareSvc.AddDMSSetting(new AddDMSSettingM.Request()
                {
                    AccID = request.AccID,
                    Logo = websetting.logo,
                    SessionKey = session.sessionkey,
                    Web_db = websetting.web_db,
                    Web_name = websetting.web_name,
                    Web_sn = websetting.web_sn
                });
                
                
                var AuthSvc = new AuthServiceClient();
                var requestUserList = allUser.Select(u => new SyncUserListM.User()
                {
                    AccID = u.email,
                    IsAdmin = u.email.Equals(websetting.web_owner),
                    AccName = u.displayname
                }).ToList();
                var syncReq = new SyncUserListM.Request()
                {
                    DBName = websetting.web_db,
                    UserList = requestUserList
                };
                AuthSvc.SyncUserList(_serviceInfo, syncReq);

                var req = new GenerateTokenM.Request() {
                    AccID = session.email,
                    DBName = websetting.web_db
                };
                var rsp = AuthSvc.GenerateToken(_serviceInfo, req);

                AuthSvc.Close();

                retResp.UserSecurityInfo = rsp.SecurityInfo;
                retResp.SessionKey = session.sessionkey;


                //if (!GlobalVariable.SessionDBMap.ContainsKey(session.sessionkey))
                //{
                //    GlobalVariable.SessionDBMap.TryAdd(session.sessionkey, websetting.web_db);
                //}

                return Result.NormalResult(request, retResp);
            }
            else
            {
                string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
                return Result.NormalResult(request, retResp, (ErrorCode)(int)ErrorCode.ArgInvalid, errorMessage);
            }
        }

        /// <summary>
        /// 定時檢核Token狀態與更新(檢查權限)
        /// 1.點擊功能時呼叫(有查詢或可合併)   2.Set TimeOut 呼叫
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<X1TokenAuthCheckM.X1TokenAuthCheckRsp> TokenAuthCheck(X1TokenAuthCheckM.X1TokenAuthCheckReq request)
        {

            X1TokenAuthCheckM.X1TokenAuthCheckRsp retResp = new X1TokenAuthCheckM.X1TokenAuthCheckRsp();
            if (ModelState.IsValid)
            {
                return await _authHandler.CheckFuncAuth(request);
            }
            else
            {
                string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
                return Result.NormalResult(request, retResp, ErrorCode.ArgInvalid, errorMessage);
            }
        }

        [HttpPost]
        public async Task<WebGetFunctionListM.Response> GetFunctionList(WebGetFunctionListM.Request request)
        {

            var retResp = new WebGetFunctionListM.Response();
            if (ModelState.IsValid)
            {
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

                var authService = new AuthServiceClient();

                var frameRequest = _frameReq.AppendDBName(new GetFunctionListM.Request()
                {
                    FuncStatus = request.FuncStatus
                });

                var frameResponse = authService.GetFunctionList(_serviceInfo, frameRequest);

                authService.Close();

                if (frameResponse.ReturnCode != 1)
                {
                    return Result.NormalResult(request, retResp, (ErrorCode)((int)ErrorCode.ProcessError + frameResponse.ReturnCode), frameResponse.ReturnMsg);
                }

                retResp.FunctionList = frameResponse.FunctionList;

                return Result.NormalResult(request, retResp);
            }
            else
            {
                string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
                return Result.NormalResult(request, retResp, ErrorCode.ArgInvalid, errorMessage);
            }
        }

        [HttpPost]
        public async Task<WebAddOrUpdateRoleM.Response> AddOrUpdateRole(WebAddOrUpdateRoleM.Request request)
        {

            var retResp = new WebAddOrUpdateRoleM.Response();
            if (ModelState.IsValid)
            {
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

                var authService = new AuthServiceClient();

                var frameRequest = _frameReq.AppendDBName(new AddOrUpdateRole.Request()
                {
                    Data = request.Data
                });
                frameRequest.Data.AccID = request.AccID;

                var frameResponse = authService.AddOrUpdateRole(_serviceInfo, frameRequest);

                authService.Close();

                if (frameResponse.ReturnCode != 1)
                {
                    return Result.NormalResult(request, retResp, (ErrorCode)((int)ErrorCode.ProcessError + frameResponse.ReturnCode), frameResponse.ReturnMsg);
                }

                retResp.RoleCode = frameResponse.RoleCode;

                return Result.NormalResult(request, retResp);
            }
            else
            {
                string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
                return Result.NormalResult(request, retResp, ErrorCode.ArgInvalid, errorMessage);
            }
        }

        [HttpPost]
        public async Task<WebGetRoleM.Response> GetRole(WebGetRoleM.Request request)
        {

            var retResp = new WebGetRoleM.Response();
            if (ModelState.IsValid)
            {
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

                var authService = new AuthServiceClient();

                var frameRequest = _frameReq.AppendDBName(new GetRoleM.Request()
                {
                    RoleCode = request.RoleCode
                });

                var frameResponse = authService.GetRole(_serviceInfo, frameRequest);

                authService.Close();

                if (frameResponse.ReturnCode != 1)
                {
                    return Result.NormalResult(request, retResp, (ErrorCode)((int)ErrorCode.ProcessError + frameResponse.ReturnCode), frameResponse.ReturnMsg);
                }

                retResp.Data = frameResponse.Data;

                return Result.NormalResult(request, retResp);
            }
            else
            {
                string errorMessage = ModelStateUtility.GetErrorMessage(ModelState);
                return Result.NormalResult(request, retResp, ErrorCode.ArgInvalid, errorMessage);
            }
           
        }

        //todo
        public void EnterByItself(string code)
       {
            
            string[] ch= { "初始"," "};
            
            string input = @"^[\u4e00-\u9fa5]+$";
            bool isCh = Regex.IsMatch(code, input);
            if (!isCh)
            {
                return;
            }
            if (code.Length >2 )
            {
                return;
            }
            if (ch[0] == "初始"&& ch[1].Length==0)
            {
                ch[1] = code;
                System.Windows.Forms.SendKeys.SendWait("{Enter}");
                return;
            }
            if (ch[0] != "初始")
            {
                return;
            }
            System.Windows.Forms.SendKeys.SendWait("{Enter}");
            ch[0] = code;
        }

    }
}
