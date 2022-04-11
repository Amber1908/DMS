using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Repository.Interface;
using X1APServer.Repository.Utility;

namespace X1APServer.Repository
{
    public class X1_ReportAnswerFileRepository : X1APBasicRepository<X1_Report_Answer_File>, IX1_ReportAnswerFileRepository
    {
        public X1_ReportAnswerFileRepository(X1APEntities dbContext) : base(dbContext)
        {
        }

        public X1_Report_Answer_File GetAnsFile(int? ID = null, int? questID = null, int? ansMId = null)
        {
            var filterData = GetAll();

            if (ID.HasValue)
            {
                filterData = filterData.Where(x => x.ID == ID);
            }

            if (questID.HasValue)
            {
                filterData = filterData.Where(x => x.QuestionID == questID);
            }

            if (ansMId.HasValue)
            {
                filterData = filterData.Where(x => x.AnswerMID == ansMId);
            }

            return filterData.FirstOrDefault();
        }
    }
}
