using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Repository.Interface;
using X1APServer.Repository.Utility;

namespace X1APServer.Repository
{
    public class X1_ReportQuestionTypeRepository : X1APBasicRepository<X1_Report_Question_Type>, IX1_ReportQuestionTypeRepository
    {
        public X1_ReportQuestionTypeRepository(X1APEntities dbContext) : base(dbContext)
        {
        }

        public X1_Report_Question_Type GetQuestType(string name)
        {
            return Get(x => x.Name.Equals(name));
        }
    }
}
