using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Repository.Interface;
using X1APServer.Repository.Utility.Interface;
using X1APServer.Service.Interface;
using X1APServer.Service.Model;

namespace X1APServer.Service
{
    public class FileService : IFileService
    {
        private IX1UnitOfWork _uow;
        private string _saveDirectoryPath = ConfigurationManager.AppSettings["UserFilePath"];

        public FileService(IX1UnitOfWork uow)
        {
            _uow = uow;

            if (!Directory.Exists(_saveDirectoryPath))
            {
                Directory.CreateDirectory(_saveDirectoryPath);
            }
        }

        public string GetFilePath(string fileName)
        {
            string filePath = Path.Combine(_saveDirectoryPath, fileName);
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("伺服器檔案不存在!");
            }

            return filePath;
        }

        public string SaveFile(string filePath, string saveFileName = "")
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("檔案不存在!");
            }

            if (string.IsNullOrEmpty(saveFileName))
            {
                saveFileName = Guid.NewGuid().ToString();
            }

            string saveFilePath = Path.Combine(_saveDirectoryPath, saveFileName);
            File.Copy(filePath, saveFilePath, true);
            return saveFilePath;
        }
    }
}
