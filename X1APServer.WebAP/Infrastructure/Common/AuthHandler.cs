using BMDC.Models.Auth;
using BMDC.Models.Log;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Unity;
using WebApplication1.Infrastructure.Common.Interface;
using WebApplication1.Infrastructure.Utility;
using X1APServer.Service.AuthService;
using X1APServer.Service.Interface;
using X1APServer.Service.Model;
using X1APServer.Service.Interface;
using static BMDC.Models.Auth.AuthCommon;

namespace WebApplication1.Infrastructure.Common
{
    public class AuthHandler
    {
        private static ServiceInfo _serviceInfo = null;
        private IIDoctorService _iDoctorSvc;
        private IDMSShareService _iDMSShareSvc;
        private IFrameRequest _frameReq;

        public enum System
        {
            X1 = 0,
            X1Web = 1
        }

        public AuthHandler(System system)
        {
            _serviceInfo = GetServiceInfo(system);
            _iDoctorSvc = DependencyResolver.Current.GetService<IIDoctorService>();
            _iDMSShareSvc = DependencyResolver.Current.GetService<IDMSShareService>();
            _frameReq = DependencyResolver.Current.GetService<IFrameRequest>();
        }

        public static ServiceInfo GetServiceInfo(System system)
        {
            ServiceInfo serviceInfo;
            string _SysCode = "";
            string _FrameServiceToken = "";

            switch (system)
            {
                case System.X1:
                    _SysCode = ConfigurationManager.AppSettings["SysCode"];
                    _FrameServiceToken = ConfigurationManager.AppSettings["FrameServiceToken"];
                    break;
                case System.X1Web:
                    _SysCode = ConfigurationManager.AppSettings["WebSysCode"];
                    _FrameServiceToken = ConfigurationManager.AppSettings["WebFrameServiceToken"];
                    break;
            }

            serviceInfo = new ServiceInfo()
            {
                SysCode = _SysCode,
                ServiceKey = _FrameServiceToken
            };
            return serviceInfo;
        }

        //彙整權限後回傳權限
        public CheckTokenAuthM.FuncAuthInfo Rebuild_AuthList(List<CheckTokenAuthM.FuncAuthInfo> authinfolist)
        {
            CheckTokenAuthM.FuncAuthInfo funcauthinfo = new CheckTokenAuthM.FuncAuthInfo()
            {
                RoleName = "",
                AuthNo1 = "N",
                AuthNo2 = "N",
                AuthNo3 = "N",
                AuthNo4 = "N",
                AuthNo5 = "N"
            };

            foreach (CheckTokenAuthM.FuncAuthInfo authinfo in authinfolist)
            {
                funcauthinfo.AuthNo1 = authinfo.AuthNo1 == "Y" ? "Y" : funcauthinfo.AuthNo1;
                funcauthinfo.AuthNo2 = authinfo.AuthNo2 == "Y" ? "Y" : funcauthinfo.AuthNo2;
                funcauthinfo.AuthNo3 = authinfo.AuthNo3 == "Y" ? "Y" : funcauthinfo.AuthNo3;
                funcauthinfo.AuthNo4 = authinfo.AuthNo4 == "Y" ? "Y" : funcauthinfo.AuthNo4;
                funcauthinfo.AuthNo5 = authinfo.AuthNo5 == "Y" ? "Y" : funcauthinfo.AuthNo5;
            }

            return funcauthinfo;
        }

        public async Task<bool> CheckSessionKey()
        {
            var sessionKey = HttpContext.Current.Request.Headers["SessionKey"];

            if (sessionKey != null)
            {
                var webSetting = _iDMSShareSvc.GetDMSSetting(sessionKey);
                return await _iDoctorSvc.CheckSessionAsync(sessionKey, webSetting.Web_sn);
            }

            return false;
        }

        //確認Token有效性 並回傳權限檢核結果
        public async Task<X1TokenAuthCheckM.X1TokenAuthCheckRsp> CheckFuncAuth<T>(T x1TokenAuthCheckReq) where T : X1APServer.Service.Model.REQBase
        {
            X1TokenAuthCheckM.X1TokenAuthCheckRsp RspObj = new X1TokenAuthCheckM.X1TokenAuthCheckRsp();
            //Input基本資料檢核
            if (string.IsNullOrWhiteSpace(x1TokenAuthCheckReq.AccID))
            {
                RspObj.ReturnCode = ErrorCode.ArgInvalid;
                RspObj.ReturnMsg = "無使用者帳號!";
                return RspObj;
            }
            if (string.IsNullOrWhiteSpace(x1TokenAuthCheckReq.UserSecurityInfo))
            {
                RspObj.ReturnCode = ErrorCode.ArgInvalid;
                RspObj.ReturnMsg = "無UserInfo可供檢核!";
                return RspObj;
            }
            if (string.IsNullOrWhiteSpace(x1TokenAuthCheckReq.FuncCode))
            {
                RspObj.ReturnCode = ErrorCode.ArgInvalid;
                RspObj.ReturnMsg = "無Function Code!";
                return RspObj;
            }
            if (!(x1TokenAuthCheckReq.AuthCode >= 1 && x1TokenAuthCheckReq.AuthCode <= 5))
            {
                RspObj.ReturnCode = ErrorCode.ArgInvalid;
                RspObj.ReturnMsg = "錯誤的AuthCode!";
                return RspObj;
            }

            //開始檢核Token&&權限
            CheckTokenAuthM.CheckAuthReq checkAuthReq = _frameReq.AppendDBName(new CheckTokenAuthM.CheckAuthReq()
            {
                AccID = x1TokenAuthCheckReq.AccID,
                UserSecurityInfo = x1TokenAuthCheckReq.UserSecurityInfo,
                FuncCode = x1TokenAuthCheckReq.FuncCode
            });

            var AuthSvc = new AuthServiceClient();

            CheckTokenAuthM.CheckAuthRsp retAuth = AuthSvc.CheckTokenAuth(_serviceInfo, checkAuthReq);
            AuthSvc.Close();
            if (!retAuth.ReturnCode.Equals(1))
            {
                RspObj.ReturnCode = (ErrorCode)((int)ErrorCode.AuthError + retAuth.ReturnCode);
                RspObj.ReturnMsg = retAuth.ReturnMsg;
                return RspObj;
            }
            if (retAuth.FuncAuths.Count.Equals(0))
            {
                RspObj.ReturnCode = ErrorCode.AuthError;
                RspObj.ReturnMsg = "您的權限不足，請重新登入系統!";
                return RspObj;
            }

            //重整權限
            var authinfo = Rebuild_AuthList(retAuth.FuncAuths);

            bool checkAuthFlag = false;
            switch (x1TokenAuthCheckReq.AuthCode)
            {
                case 1:
                    if (authinfo.AuthNo1 == "Y") checkAuthFlag = true;
                    break;
                case 2:
                    if (authinfo.AuthNo2 == "Y") checkAuthFlag = true;
                    break;
                case 3:
                    if (authinfo.AuthNo3 == "Y") checkAuthFlag = true;
                    break;
                case 4:
                    if (authinfo.AuthNo4 == "Y") checkAuthFlag = true;
                    break;
                case 5:
                    if (authinfo.AuthNo5 == "Y") checkAuthFlag = true;
                    break;
                default:
                    break;
            }
            if (!checkAuthFlag)
            {
                RspObj.ReturnCode = ErrorCode.AuthError;
                RspObj.ReturnMsg = "您的權限不足，請重新登入系統!";
                return RspObj;
            }
            //確認是否需更新Token(UserInfo)
            if (retAuth.TokenChgFlag)
            {
                RspObj.TokenChgFlag = true;
                RspObj.UserSecurityInfo = retAuth.UserSecurityInfo;
            }

            if (!await CheckSessionKey())
            {
                RspObj.ReturnCode = ErrorCode.SessionKeyInvalid;
                RspObj.ReturnMsg = "該帳戶已由其他裝置進行登入，請檢查帳號是否外洩";
                return RspObj;
            }

            RspObj.ReturnCode = ErrorCode.OK;
            RspObj.ReturnMsg = "OK";
            return RspObj;
        }
    }
}