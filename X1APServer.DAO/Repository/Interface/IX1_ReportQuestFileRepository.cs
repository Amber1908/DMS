using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Repository.Utility.Interface;

namespace X1APServer.Repository.Interface
{
    public interface IX1_ReportQuestFileRepository : IBasicRepository<X1_Report_Question_File>
    {
        /// <summary>
        /// 刪除所有同個 Report Main 的檔案
        /// </summary>
        /// <param name="RMID"></param>
        void DeleteAllByReportMID(int RMID);
        /// <summary>
        /// 取得 Report Quest File
        /// </summary>
        /// <param name="RQID"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        X1_Report_Question_File GetReportQuestFile(int? RQID, string fileName);
    }
}
