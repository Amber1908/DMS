using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Repository.Utility.Interface;

namespace X1APServer.Repository.Interface
{
    public interface IX1_ReportAnswerFileRepository : IBasicRepository<X1_Report_Answer_File>
    {
        X1_Report_Answer_File GetAnsFile(int? ID = null, int? questID = null, int? ansMId = null);
    }
}
