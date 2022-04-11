using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Repository.Interface;
using X1APServer.Repository.Utility;

namespace X1APServer.Repository
{
    public class X1_ReportQuestFileRepository : X1APBasicRepository<X1_Report_Question_File>, IX1_ReportQuestFileRepository
    {
        public X1_ReportQuestFileRepository(X1APEntities dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// 刪除所有Report Main ID的檔案
        /// </summary>
        /// <param name="RMID"></param>
        public void DeleteAllByReportMID(int RMID)
        {
            Delete(x => x.RMID == RMID);
        }

        public X1_Report_Question_File GetReportQuestFile(int? RQID, string fileName)
        {
            var filterResult = GetAll();

            if (RQID.HasValue)
            {
                filterResult = filterResult.Where(x => x.RQID == RQID.Value);
            }

            if (!string.IsNullOrEmpty(fileName))
            {
                filterResult = filterResult.Where(x => x.FileName.Equals(fileName));
            }

            return filterResult.FirstOrDefault();
        }
    }
}
