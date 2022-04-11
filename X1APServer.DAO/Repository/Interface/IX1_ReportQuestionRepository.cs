using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Repository.Utility.Interface;

namespace X1APServer.Repository.Interface
{
    public interface IX1_ReportQuestionRepository : IBasicRepository<X1_Report_Question>
    {
        IEnumerable<X1_Report_Question> GetAllQuestions(int reportId);

        X1_Report_Question GetQuest(int? id = null, string questNo = null);
    }
}
