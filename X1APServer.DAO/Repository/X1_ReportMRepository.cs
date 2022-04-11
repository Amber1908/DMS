using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Repository.Interface;
using X1APServer.Repository.Utility;

namespace X1APServer.Repository
{
    public class X1_ReportMRepository : X1APBasicRepository<X1_Report_Main>, IX1_ReportMRepository
    {
        public X1_ReportMRepository(X1APEntities dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// 取得所有不同類別最新的 Report Main
        /// </summary>
        /// <param name="isPublish"></param>
        /// <returns></returns>
        public IQueryable<X1_Report_Main> GetAllCategoryLatestReportM(bool? isPublish = null)
        {
            // 取最新建立的Report
            var filterReportMList = GetAll().Where(x => !x.IsDelete);

            if (isPublish.HasValue)
            {
                filterReportMList = filterReportMList.Where(x => x.IsPublish == isPublish.Value && x.ReserveDate <= DateTime.Now);
            }

            var latestReportId = filterReportMList
                .GroupBy(x => x.Category)
                .Select(x => x.Max(y => y.ID)).ToList();

            var reportMList = GetAll().Where(x => latestReportId.Contains(x.ID));
            
            return reportMList;
        }
        
        /// <summary>
        /// 取得所有版本的Report
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public IQueryable<X1_Report_Main> GetAllVersionReportM(string category)
        {
            var reportMs = GetAll().OrderByDescending(x => x.CreateDate).Where(x => x.Category.Equals(category) && !x.IsDelete);
            return reportMs;
        }

        public IQueryable<X1_Report_Main> GetAllWithQuestion()
        {
            return GetAll().Where(m => !m.IsDelete)
                .Include(m => m.X1_Report_Question);
        }

        public X1_Report_Main GetLatestReportM(string category = null, int? id = null, bool? isPublish = null)
        {
            var temp = GetAll().OrderByDescending(x => x.CreateDate).Where(x => !x.IsDelete);

            if (!string.IsNullOrEmpty(category))
            {
                temp = temp.Where(x => x.Category.Equals(category));
            }

            if (id.HasValue)
            {
                temp = temp.Where(x => x.ID == id);
            }

            if (isPublish.HasValue)
            {
                temp = temp.Where(x => x.IsPublish == isPublish.Value);
            }

            var reportM = temp.FirstOrDefault();
            return reportM;
        }

        
    }
}
