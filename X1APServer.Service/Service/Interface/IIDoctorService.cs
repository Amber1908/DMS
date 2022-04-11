using iDoctorTools.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Service.Model;

namespace X1APServer.Service.Interface
{
    public interface IIDoctorService
    {
        /// <summary>
        /// 取出會員登入暫存資料.
        /// </summary>
        /// <param name="pusid"></param>
        /// <returns></returns>
        Task<PUSID> PopPusid(string pusid);
        /// <summary>
        /// 檢核登入 Session
        /// </summary>
        /// <param name="sessionkey"></param>
        /// <param name="web_sn"></param>
        /// <returns></returns>
        Task<bool> CheckSessionAsync(string sessionkey, int web_sn);
        /// <summary>
        /// 查詢會員詳細資料
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<USER> GetUser(string email);
        /// <summary>
        /// 取得病程網站詳細資料
        /// </summary>
        /// <param name="web_sn"></param>
        /// <returns></returns>
        Task<HEALTHWEB> GetHealthWeb(int web_sn);
        /// <summary>
        /// 查詢會員資料 By Health Web
        /// </summary>
        Task<List<USER>> GetUserByHealthWeb(int web_sn);
        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<ActionResultModel> UserLogin(string email, string password);
        /// <summary>
        /// 產生Session
        /// </summary>
        /// <param name="email"></param>
        /// <param name="web_sn"></param>
        /// <returns></returns>
        Task<SESSION> GenSession(string email, int web_sn);
        /// <summary>
        /// 取得病程網站 By User
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<GetHealthWebByUserM.Response> GetHealthWebByUser(string email);
        /// <summary>
        /// 更新使用者密碼
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="newpassword"></param>
        /// <returns></returns>
        Task<RSPBase> UserChangePassword(string email, string password, string newpassword);
        /// <summary>
        /// 取得地區代碼表
        /// </summary>
        /// <returns></returns>
        Task<List<AREACODE>> GetAreaCode();
        /// <summary>
        /// 取得醫療機構代碼表
        /// </summary>
        /// <param name="code">代碼開頭篩選</param>
        /// <returns></returns>
        Task<List<HOSPITALCODE>> GetHospitalCode(string code);
        /// <summary>
        /// 取得醫療機構代碼表字串
        /// </summary>
        /// <param name="code">代碼開頭篩選</param>
        /// <returns></returns>
        Task<List<HOSPITALCODELAZY>> GetHospitalCodeLazy(string code);
    }
}
