using X1APServer.Service.Model;

namespace X1APServer.Service.Interface
{
    public interface IFileService
    {
        /// <summary>
        /// 儲存檔案
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="saveFileName"></param>
        /// <returns></returns>
        string SaveFile(string filePath, string saveFileName = "");
        /// <summary>
        /// 取得檔案路徑
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        string GetFilePath(string fileName);
    }
}
