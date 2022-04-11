using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Repository.Interface;
using X1APServer.Repository.Utility;

namespace X1APServer.Repository
{
    public class X1_ReportAnswerDRepository : X1APBasicRepository<X1_Report_Answer_Detail>, IX1_ReportAnswerDRepository
    {
        public X1_ReportAnswerDRepository(X1APEntities dbContext) : base(dbContext)
        {
            
        }

        public IQueryable<X1_Report_Answer_Detail> GetAllAns(int? ansMID)
        {
            var filterAns = GetAll();

            if (ansMID.HasValue)
            {
                filterAns = filterAns.Where(x => x.AnswerMID == ansMID);
            }

            return filterAns;
        }

        public X1_Report_Answer_Detail GetAns(int? id, int? questID)
        {
            var filterAns = GetAll();

            if (id.HasValue)
            {
                filterAns = filterAns.Where(x => x.ID == id.Value);
            }

            if (questID.HasValue)
            {
                filterAns = filterAns.Where(x => x.QuestionID == questID);
            }

            return filterAns.FirstOrDefault();
        }
    }
}
