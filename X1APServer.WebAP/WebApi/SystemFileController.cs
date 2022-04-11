using NLog;
using System;
using System.Collections.Generic;
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
using X1APServer.Service.Interface;
using X1APServer.Service.Model;

namespace WebApplication1.WebApi
{
    public class SystemFileController : ApiController
    {
        private ISystemFileService _svc;
        private AuthHandler _authHandler = null;

        public SystemFileController(ISystemFileService svc)
        {
            _svc = svc;
            _authHandler = new AuthHandler(AuthHandler.System.X1Web);
        }

        /// <summary>
        /// 上傳檔案
        /// </summary>
        /// <returns></returns>
        [IgnoreRequestContent]
        public async Task<AddFileM.Response> AddFile()
        {
            var request = new AddFileM.Request();
            var retResp = new AddFileM.Response();

            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = HttpContext.Current.Server.MapPath("~/App_Data/Temp");
            Directory.CreateDirectory(root);
            var provider = new MultipartFormDataStreamProvider(root);

            await Request.Content.ReadAsMultipartAsync(provider);

            try
            {
                request.AccID = provider.FormData["AccID"];
                request.FuncCode = provider.FormData["FuncCode"];
                request.AuthCode = int.Parse(provider.FormData["AuthCode"]);
                request.UserSecurityInfo = provider.FormData["UserSecurityInfo"];
                request.FilePath = provider.FileData[0].LocalFileName.Replace("\"", "");
                request.FileName = provider.FileData[0].Headers.ContentDisposition.FileName.Replace("\"", "");
                request.MimeType = provider.FileData[0].Headers.ContentType.MediaType.Replace("\"", "");
            }
            catch (Exception e)
            {
                return Result.NormalResult(request, retResp, ErrorCode.ArgInvalid, e.Message);
            }

            //檢核權限
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

            // 新增檔案
            string rootPath = System.Web.Hosting.HostingEnvironment.MapPath("~");
            var serviceRsp = _svc.AddFile(request, ref retResp, rootPath);
            if (serviceRsp.ReturnCode != ErrorCode.OK)
            {
                return Result.NormalResult(request, retResp, ErrorCode.ProcessError, serviceRsp.ReturnMsg);
            }

            return Result.NormalResult(request, retResp);
        }


        /// <summary>
        /// 上傳檔案
        /// </summary>
        /// <returns></returns>
        [IgnoreRequestContent]
        public async Task<UpdateFileM.Response> UpdateFile()
        {
            var request = new UpdateFileM.Request();
            var retResp = new UpdateFileM.Response();

            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = HttpContext.Current.Server.MapPath("~/App_Data/Temp");
            Directory.CreateDirectory(root);
            var provider = new MultipartFormDataStreamProvider(root);

            await Request.Content.ReadAsMultipartAsync(provider);

            try
            {
                request.AccID = provider.FormData["AccID"];
                request.FuncCode = provider.FormData["FuncCode"];
                request.AuthCode = int.Parse(provider.FormData["AuthCode"]);
                request.UserSecurityInfo = provider.FormData["UserSecurityInfo"];
                request.ID = new Guid(provider.FormData["ID"]);
                request.FilePath = provider.FileData[0].LocalFileName.Replace("\"", "");
                request.FileName = provider.FileData[0].Headers.ContentDisposition.FileName.Replace("\"", "");
                request.MimeType = provider.FileData[0].Headers.ContentType.MediaType.Replace("\"", "");
            }
            catch (Exception e)
            {
                return Result.NormalResult(request, retResp, ErrorCode.ArgInvalid, e.Message);
            }

            //檢核權限
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

            // 更新檔案
            string rootPath = System.Web.Hosting.HostingEnvironment.MapPath("~");
            var serviceRsp = _svc.UpdateFile(request, ref retResp, rootPath);
            if (serviceRsp.ReturnCode != ErrorCode.OK)
            {
                return Result.NormalResult(request, retResp, ErrorCode.ProcessError, serviceRsp.ReturnMsg);
            }

            return Result.NormalResult(request, retResp);
        }
    }
}
