using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Repository.Interface
{
    public interface IX1_ReportAnswerDRepository : Utility.Interface.IBasicRepository<X1_Report_Answer_Detail>
    {
        X1_Report_Answer_Detail GetAns(int? id, int? questID);

        IQueryable<X1_Report_Answer_Detail> GetAllAns(int? ansMID);
    }
}
