using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Repository.Interface;
using X1APServer.Repository.Utility;

namespace X1APServer.Repository
{
    public class X1_ReportAnswerExtraRepository : X1APBasicRepository<X1_Report_Answer_Extra>, IX1_ReportAnswerExtraRepository
    {
        public X1_ReportAnswerExtraRepository(X1APEntities dbContext) : base(dbContext)
        {
        }
    }
}
