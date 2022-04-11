using iDoctorTools.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Repository;
using X1APServer.Service.Model;

namespace X1APServer.Service.Interface
{
    public interface IDMSShareService
    {
        /// <summary>
        /// 取得網站設定資料
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <returns></returns>
        DMSSetting GetDMSSetting(string sessionKey);
        /// <summary>
        /// 取得網站設定資料
        /// </summary>
        /// <param name="WebSN"></param>
        /// <returns></returns>
        DMSSetting GetDMSSettingBySN(int WebSN);
        /// <summary>
        /// 新增網站設定資料及session key對應
        /// </summary>
        /// <param name="dmsSetting"></param>
        /// <param name="sessionKey"></param>
        /// <returns></returns>
        void AddDMSSetting(AddDMSSettingM.Request request);
        /// <summary>
        /// 取得session key
        /// </summary>
        /// <param name="accid"></param>
        /// <returns></returns>
        SessionDMSMap GetSessionKeyMap(string accid);
        /// <summary>
        /// 新增token
        /// </summary>
        /// <param name="sessionkey"></param>
        /// <returns></returns>
        UserToken AddUserToken(string accid);
        /// <summary>
        /// 檢查token
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        bool CheckToken(string accid, string token);
    }
}
