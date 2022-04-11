
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using WebApplication1.ViewModels;
using System.Configuration;
using System.Threading.Tasks;
using X1APServer.Service.Interface;
using WebApplication1.Infrastructure.Common;
using BMDC.Models.Auth;
using Newtonsoft.Json;
using X1APServer.Service.AuthService;
using X1APServer.Service.Interface;
using X1APServer.Service.Model;

namespace X1APServer.Controllers
{
    //[OutputCache(CacheProfile = "WebPage")]
    public class HomeController : Controller
    {
        private IIDoctorService _idoctorSvc;
        private IDMSShareService _dmsShareSvc;

        public HomeController(IIDoctorService idoctorSvc, IDMSShareService dmsShareSvc)
        {
            _idoctorSvc = idoctorSvc;
            _dmsShareSvc = dmsShareSvc;
        }

        public async Task<ActionResult> Index(string pusid)
        {
            var webSetting = await _idoctorSvc.PopPusid(pusid);

            if (webSetting != null)
            {
                var allUser = await _idoctorSvc.GetUserByHealthWeb(webSetting.web_sn);
                var webSettingDetail = await _idoctorSvc.GetHealthWeb(webSetting.web_sn);
                var client = new AuthServiceClient();
                var serviceInfo = AuthHandler.GetServiceInfo(AuthHandler.System.X1Web);
                var requestUserList = allUser.Select(u => new SyncUserListM.User()
                {
                    AccID = u.email,
                    IsAdmin = u.email.Equals(webSettingDetail.web_owner),
                    AccName = u.displayname
                }).ToList();
                var request = new SyncUserListM.Request() {
                    DBName = webSetting.web_db,
                    UserList = requestUserList
                };
                client.SyncUserList(serviceInfo, request);

                var genTokenReq = new GenerateTokenM.Request()
                {
                    AccID = webSetting.email,
                    DBName = webSetting.web_db
                };
                var genTokenRsp = client.GenerateToken(serviceInfo, genTokenReq);
                client.Close();

                var addDMSSettingReq = new AddDMSSettingM.Request()
                {
                    AccID = webSetting.email,
                    Logo = webSetting.logo,
                    SessionKey = webSetting.sessionkey,
                    Web_db = webSetting.web_db,
                    Web_name = webSetting.web_name,
                    Web_sn = webSetting.web_sn
                };
                _dmsShareSvc.AddDMSSetting(addDMSSettingReq);

                var cookieContent = JsonConvert.SerializeObject(new
                {
                    AccID = webSetting.email,
                    UserSecurityInfo = genTokenRsp.SecurityInfo,
                    SessionKey = webSetting.sessionkey,
                    WebSn = webSetting.web_sn.ToString()
                });
                Response.Cookies.Clear();
                var cookie = new HttpCookie("SecurityInfo", cookieContent);
                cookie.Expires = DateTime.Now.AddHours(24);
                Response.Cookies.Add(cookie);
            }
            
            return View();
        }

        public ActionResult ReportViewer(string pid)
        {
            int pidNum = -1;
            int.TryParse(pid, out pidNum);

            if (pidNum == -1)
            {
                return HttpNotFound();
            }

            return View(new ReportViewerViewModel() { PID = pidNum });
        }

        public ActionResult PlayGround()
        {
            return View();
        }

        public ActionResult ReportPDF(string id)
        {
            string manageFolder = ConfigurationManager.AppSettings["ManagedFolderPath"];
            string sourcePath = Path.Combine(manageFolder, "ExportFile", id);

            string dirPath = Server.MapPath("~/Content/Temp/");
            string filePath = Path.Combine(dirPath, id);

            System.IO.File.Copy(sourcePath, filePath, true);
            // 檢查檔案是否存在
            if (!System.IO.File.Exists(filePath))
            {
                return HttpNotFound();
            }

            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.DeleteOnClose);
            
            return File(fileStream, "application/pdf", id + ".pdf");
        }
    }
}
