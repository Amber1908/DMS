using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Repository.Utility.Interface;

namespace X1APServer.Repository.Interface
{
    public interface IX1_ReportAuthRepository : IBasicRepository<X1_Report_Authorization>
    {
        /// <summary>
        /// 取得所有權限
        /// </summary>
        /// <param name="reportID"></param>
        /// <returns></returns>
        IQueryable<X1_Report_Authorization> GetAllAuth(int? reportID);
    }
}
