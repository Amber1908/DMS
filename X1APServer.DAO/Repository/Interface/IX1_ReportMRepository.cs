using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Repository.Utility.Interface;

namespace X1APServer.Repository.Interface
{
    public interface IX1_ReportMRepository : IBasicRepository<X1_Report_Main>
    {
        X1_Report_Main GetLatestReportM(string category = null, int? id = null, bool? isPublish = null);

        /// <summary>
        /// 取得所有不同類別最新的 Report Main
        /// </summary>
        /// <param name="isPublish"></param>
        /// <returns></returns>
        IQueryable<X1_Report_Main> GetAllCategoryLatestReportM(bool? isPublish = null);
        /// <summary>
        /// 取得所有版本的Report
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        IQueryable<X1_Report_Main> GetAllVersionReportM(string category);

        IQueryable<X1_Report_Main> GetAllWithQuestion();
    }
}
