using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Repository;
using X1APServer.Repository.Interface;
using X1APServer.Repository.Utility.Interface;
using X1APServer.Service.Interface;
using X1APServer.Service.Model;
using X1APServer.Service.Utils;

namespace X1APServer.Service
{
    public class SystemFileService : ISystemFileService
    {
        private IX1UnitOfWork _uow;
        private readonly string fileStorePath = "App_Data\\Files";

        public SystemFileService(IX1UnitOfWork uow)
        {
            _uow = uow;
        }

        public RSPBase AddFile(AddFileM.Request request, ref AddFileM.Response response, string rootPath)
        {
            if (!File.Exists(request.FilePath))
            {
                return ResponseHelper.CreateResponse(ErrorCode.NotFound, "檔案上傳錯誤");
            }

            try
            {
                _uow.BeginRootTransaction();

                Guid id = Guid.NewGuid();
                string newFileName = id.ToString();
                string storeDirPath = Path.Combine(rootPath, fileStorePath);
                string storePath = Path.Combine(storeDirPath, newFileName);
                string storeRelativePath = Path.Combine(fileStorePath, newFileName);

                Directory.CreateDirectory(storeDirPath);

                var insertFile = new SystemFile()
                {
                    ID = id,
                    FilePath = storeRelativePath,
                    FileName = request.FileName,
                    MimeType = request.MimeType
                };
                _uow.Get<ISystemFileRepository>().Create(insertFile, request.AccID);
                _uow.Commit();
                
                File.Copy(request.FilePath, storePath, true);

                _uow.CommitRootTransaction();
            }
            catch (Exception e)
            {
                _uow.RollBackRootTransaction();
                throw;
            }

            return ResponseHelper.Ok();
        }

        public RSPBase DeleteFile(DeleteFileM.Reqeust request, ref DeleteFileM.Response response)
        {
            var fileRepo = _uow.Get<ISystemFileRepository>();
            var file = fileRepo.GetFile(request.ID);
            if (file == null)
            {
                return ResponseHelper.CreateResponse(ErrorCode.NotFound, "無此檔案ID: " + request.ID.ToString());
            }

            fileRepo.SoftDelete(file, request.AccID);
            _uow.Commit();

            return ResponseHelper.Ok();
        }

        public RSPBase GetFile(GetFileM.Request request, ref GetFileM.Response response)
        {
            var file = _uow.Get<ISystemFileRepository>().GetFile(request.ID);
            if (file == null)
            {
                return ResponseHelper.CreateResponse(ErrorCode.NotFound, "無此檔案ID: " + request.ID.ToString());
            }

            response.FileName = file.FileName;
            response.FilePath = file.FilePath;
            response.MimeType = file.MimeType;

            return ResponseHelper.Ok();
        }

        public RSPBase UpdateFile(UpdateFileM.Request request, ref UpdateFileM.Response response, string rootPath)
        {
            if (!File.Exists(request.FilePath))
            {
                return ResponseHelper.CreateResponse(ErrorCode.NotFound, "檔案上傳錯誤");
            }

            var fileRepo = _uow.Get<ISystemFileRepository>();
            var file = fileRepo.GetFile(request.ID);
            if (file == null)
            {
                return ResponseHelper.CreateResponse(ErrorCode.NotFound, "無此檔案ID: " + request.ID);
            }

            try
            {
                _uow.BeginRootTransaction();
                
                string storePath = Path.Combine(rootPath, fileStorePath, file.ID.ToString());

                file.FileName = request.FileName;
                file.MimeType = request.MimeType;
                _uow.Get<ISystemFileRepository>().Update(file, request.AccID);
                _uow.Commit();

                File.Copy(request.FilePath, storePath, true);

                _uow.CommitRootTransaction();
            }
            catch (Exception)
            {
                _uow.RollBackRootTransaction();
                throw;
            }

            return ResponseHelper.Ok();
        }
    }
}
