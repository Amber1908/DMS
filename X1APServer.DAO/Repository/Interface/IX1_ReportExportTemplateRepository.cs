using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Repository.Utility.Interface;

namespace X1APServer.Repository.Interface
{
    public interface IX1_ReportExportTemplateRepository : IBasicRepository<X1_Report_Export_Template>
    {
        /// <summary>
        /// 取得報告模板清單
        /// </summary>
        /// <returns></returns>
        IQueryable<X1_Report_Export_Template> GetTemplateList();
        /// <summary>
        /// 取得報告模板
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        X1_Report_Export_Template GetTemplate(int id);
    }
}
