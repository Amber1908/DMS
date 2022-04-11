using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Repository.Interface;
using X1APServer.Repository.Utility;

namespace X1APServer.Repository
{
    public class X1_ReportExportTemplateRepository : X1APBasicRepository<X1_Report_Export_Template>, IX1_ReportExportTemplateRepository
    {
        public X1_ReportExportTemplateRepository(X1APEntities dbContext) : base(dbContext)
        {
        }

        public X1_Report_Export_Template GetTemplate(int id)
        {
            var filterTemplate = GetAll().Where(x => !x.IsDelete);

            filterTemplate = filterTemplate.Where(x => id == x.ID);

            return filterTemplate.FirstOrDefault();
        }

        public IQueryable<X1_Report_Export_Template> GetTemplateList()
        {
            var filterTemplate = GetAll().Where(x => !x.IsDelete);

            return filterTemplate;
        }
    }
}
