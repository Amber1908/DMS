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
    public class X1_ReportAnswerMRepository : X1APBasicRepository<X1_Report_Answer_Main>, IX1_ReportAnswerMRepository
    {
        public X1_ReportAnswerMRepository(X1APEntities dbContext) : base(dbContext)
        {
        }

        public IQueryable<X1_Report_Answer_Main> GetAllReport(int reportID)
        {
            var allReportAnsM = GetAll().Where(x => !x.IsDelete);

            if (reportID != null)
            {
                allReportAnsM = allReportAnsM.Where(x => x.ReportID == reportID);
            }

            return allReportAnsM;
        }

        public X1_Report_Answer_Main GetLatestReport(int reportID, int patientID, DateTime? specificDate = null)
        {
            var reportM = GetAll().Where(x => !x.IsDelete && x.ReportID == reportID && x.PID == patientID);

            if (specificDate.HasValue)
            {
                reportM = reportM.Where(x => x.CreateDate <= specificDate);
            }

            return reportM.OrderByDescending(x => x.CreateDate).FirstOrDefault();
        }

        public X1_Report_Answer_Main GetMaxFillingDateReport(int reportID, int patientID, DateTime? specificDate = null)
        {
            var reportM = GetAll().Where(x => !x.IsDelete && x.ReportID == reportID && x.PID == patientID);

            if (specificDate.HasValue)
            {
                reportM = reportM.Where(x => x.FillingDate <= specificDate);
            }

            return reportM.OrderByDescending(x => x.FillingDate).FirstOrDefault();
        }

        public X1_Report_Answer_Main GetReport(int? id, int? reportID)
        {
            var allReportAnsM = GetAll().Where(x => !x.IsDelete);

            if (id.HasValue)
            {
                allReportAnsM = allReportAnsM.Where(x => x.ID == id);
            }

            if (reportID.HasValue)
            {
                allReportAnsM = allReportAnsM.Where(x => x.ReportID == reportID.Value);
            }

            return allReportAnsM.FirstOrDefault();
        }

        //public Tuple<int, string> GetSequenceNum(DateTime? date = null)
        //{
        //    if (!date.HasValue)
        //    {
        //        date = DateTime.Now;
        //    }
            
        //    var ansM = GetAll().Where(m => DbFunctions.DiffDays(m.FillingDate, date) == 0)
        //                    .OrderByDescending(m => m.SequenceNum).FirstOrDefault();
        //    int seqId = 1;

        //    if (ansM != null)
        //    {
        //        var seqIdStr = ansM.SequenceNum.Substring(8, 3);
        //        seqId = int.Parse(seqIdStr) + 1;
        //    }

        //    var result = date.Value.ToString("yyyyMMdd") + seqId.ToString().PadLeft(3, '0');
        //    return new Tuple<int, string>(seqId, result);
        //}
    }
}
