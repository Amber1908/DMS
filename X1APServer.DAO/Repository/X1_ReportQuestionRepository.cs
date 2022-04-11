using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Repository.Interface;
using X1APServer.Repository.Utility;

namespace X1APServer.Repository.Repository
{
    public class X1_ReportQuestionRepository : X1APBasicRepository<X1_Report_Question>, IX1_ReportQuestionRepository
    {
        public X1_ReportQuestionRepository(X1APEntities dbContext) : base(dbContext)
        {
        }

        public IEnumerable<X1_Report_Question> GetAllQuestions(int reportId)
        {
            var allQuests = GetAll();

            allQuests = allQuests.Where(x => x.ReportID == reportId);

            return allQuests.AsEnumerable();
        }

        public X1_Report_Question GetQuest(int? id = null, string questNo = null)
        {
            var quest = GetAll();

            if (id.HasValue)
            {
                quest = quest.Where(q => q.ID == id);
            }

            if (!string.IsNullOrEmpty(questNo))
            {
                quest = quest.Where(q => q.QuestionNo == questNo);
            }

            return quest.FirstOrDefault();
        }
    }
}
