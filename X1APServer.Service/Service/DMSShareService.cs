using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using iDoctorTools.Models;
using Newtonsoft.Json;
using X1APServer.Repository;
using X1APServer.Repository.Interface;
using X1APServer.Repository.Utility.Interface;
using X1APServer.Service.Interface;
using X1APServer.Service.Misc;
using X1APServer.Service.Model;
using X1APServer.Service.Utils;

namespace X1APServer.Service
{
    public class DMSShareService : IDMSShareService
    {
        private readonly IDMSShareUnitOfWork _uow;
        private readonly IIDoctorService _idoctorSvc;

        public DMSShareService(IDMSShareUnitOfWork uow, IIDoctorService idoctorSvc)
        {
            _uow = uow;
            _idoctorSvc = idoctorSvc;
        }

        public void AddDMSSetting(AddDMSSettingM.Request request)
        {
            try
            {
                _uow.BeginTransaction();

                var sessionMapRepo = _uow.Get<ISessionDMSMapRepository>();
                var dmsMapList = sessionMapRepo.GetAll().Where(s => s.AccID == request.AccID).ToList();
                foreach (var dmsMap in dmsMapList)
                {
                    sessionMapRepo.Delete(dmsMap);
                }
                _uow.Commit();

                var sessionMap = new SessionDMSMap()
                {
                    AccID = request.AccID,
                    Sessionkey = request.SessionKey,
                    Web_sn = request.Web_sn
                };
                sessionMapRepo.Create(sessionMap);

                var dmsSettingRepo = _uow.Get<IDMSSettingRepository>();
                var websetting = dmsSettingRepo.Get(w => w.Web_sn == request.Web_sn);
                if (websetting == null)
                {
                    websetting = new DMSSetting()
                    {
                        Logo = request.Logo,
                        Web_db = request.Web_db,
                        Web_name = request.Web_name,
                        Web_sn = request.Web_sn
                    };
                    dmsSettingRepo.Create(websetting);
                }
                else
                {
                    websetting.Web_name = request.Web_name;
                    websetting.Logo = request.Logo;
                    dmsSettingRepo.Update(websetting);
                }

                _uow.Commit();

                _uow.CommitTransaction();
            }
            catch (Exception e)
            {
                _uow.RollBackTransaction();
                throw;
            }
        }

        public UserToken AddUserToken(string accid)
        {
            var sessionRepo = _uow.Get<IUserTokenRepository>();
            var session = sessionRepo.Get(x => x.AccID == accid);
            if (session == null)
            {
                session = new UserToken()
                {
                    AccID = accid,
                    Token = Guid.NewGuid()
                };
                sessionRepo.Create(session);
            }
            else
            {
                session.Token = Guid.NewGuid();
                sessionRepo.Update(session);
            }

            _uow.Commit();
            return session;
        }

        public bool CheckToken(string accid, string token)
        {
            var userTokenRepo = _uow.Get<IUserTokenRepository>();
            var usertoken = userTokenRepo.Get(x => x.AccID == accid);

            if (usertoken == null || !usertoken.Token.Equals(new Guid(token)))
            {
                return false;
            }
            else
            {
                userTokenRepo.Delete(usertoken);
                _uow.Commit();
                return true;
            }
        }

        public DMSSetting GetDMSSetting(string sessionKey)
        {
            var sessionMap = _uow.Get<ISessionDMSMapRepository>().Get(s => s.Sessionkey == sessionKey);
            if (sessionMap == null)
            {
                return null;
            }

            var dmsSetting = _uow.Get<IDMSSettingRepository>().Get(s => s.Web_sn == sessionMap.Web_sn);
            if (dmsSetting == null)
            {
                return null;
            }

            return dmsSetting;
        }

        public DMSSetting GetDMSSettingBySN(int WebSN)
        {
            var dmsSetting = _uow.Get<IDMSSettingRepository>().Get(s => s.Web_sn == WebSN);
            if (dmsSetting == null)
            {
                return null;
            }

            return dmsSetting;
        }

        public SessionDMSMap GetSessionKeyMap(string accid)
        {
            var sessionKey = _uow.Get<ISessionDMSMapRepository>().Get(x => x.AccID == accid);
            return sessionKey;
        }
    }
}
