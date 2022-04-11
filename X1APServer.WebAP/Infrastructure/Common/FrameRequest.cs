using BMDC.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Unity;
using WebApplication1.Infrastructure.Common;
using WebApplication1.Infrastructure.Common.Interface;
using X1APServer.Service.Interface;

namespace WebApplication1.Infrastructure.Common
{
    public class FrameRequest : IFrameRequest
    {
        private IDMSShareService _svc;

        public FrameRequest(IDMSShareService svc)
        {
            _svc = svc;
        }

        public T AppendDBName<T>(T request) where T : REQBase
        {
            var sessionKey = HttpContext.Current.Request.Headers["SessionKey"];
            if (!GlobalVariable.Instance.ContainsKey(sessionKey))
            {
                var dmsSetting = _svc.GetDMSSetting(sessionKey);
                GlobalVariable.Instance.TryAdd(sessionKey, dmsSetting.Web_db);
            }

            request.DBName = GlobalVariable.Instance.Get(sessionKey);
            return request;
        }

        public string GetDBName()
        {
            var sessionKey = HttpContext.Current.Request.Headers["SessionKey"];
            if (!GlobalVariable.Instance.ContainsKey(sessionKey))
            {
                var dmsSetting = _svc.GetDMSSetting(sessionKey);
                GlobalVariable.Instance.TryAdd(sessionKey, dmsSetting.Web_db);
            }
            return GlobalVariable.Instance.Get(sessionKey);
        }
    }
}