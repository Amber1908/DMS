
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Misc;
using Newtonsoft.Json;
using System.IO;
using WebApplication1.ViewModels;
using System.Configuration;
using X1APServer.Service.Interface;

namespace X1APServer.Controllers
{
    public class FileController : Controller
    {
        private ISystemFileService _svc;

        public FileController(ISystemFileService svc)
        {
            _svc = svc;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Get(string id)
        {
            var request = new Service.Model.GetFileM.Request()
            {
                ID = new Guid(id)
            };
            var response = new Service.Model.GetFileM.Response();
            _svc.GetFile(request, ref response);
            string filePath = Server.MapPath("~/" + response.FilePath);

            // 檢查檔案是否存在
            if (!System.IO.File.Exists(filePath))
            {
                return HttpNotFound();
            }

            Response.AppendHeader("Filename", response.FileName);
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096);

            return File(fileStream, response.MimeType, response.FileName);
        }
    }
}
