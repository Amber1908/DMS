using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Repository.Utility.Interface;

namespace X1APServer.Repository.Interface
{
    public interface IX1_ReportAnswerMRepository : IBasicRepository<X1_Report_Answer_Main>
    {
        X1_Report_Answer_Main GetLatestReport(int reportID, int patientID, DateTime? specificDate = null);

        X1_Report_Answer_Main GetMaxFillingDateReport(int reportID, int patientID, DateTime? specificDate = null);

        X1_Report_Answer_Main GetReport(int? id, int? reportID);

        IQueryable<X1_Report_Answer_Main> GetAllReport(int reportID);

        //Tuple<int, string> GetSequenceNum(DateTime? date = null);
    }
}
