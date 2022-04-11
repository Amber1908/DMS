using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Service.Model;

namespace X1APServer.Service.Interface
{
    public interface ISystemFileService
    {
        /// <summary>
        /// 新增檔案
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        RSPBase AddFile(AddFileM.Request request, ref AddFileM.Response response, string rootPath);
        /// <summary>
        /// 更新檔案
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        RSPBase UpdateFile(UpdateFileM.Request request, ref UpdateFileM.Response response, string rootPath);
        /// <summary>
        /// 刪除檔案
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        RSPBase DeleteFile(DeleteFileM.Reqeust request, ref DeleteFileM.Response response);
        /// <summary>
        /// 取得檔案
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        RSPBase GetFile(GetFileM.Request request, ref GetFileM.Response response);
    }
}
