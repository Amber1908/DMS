using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Repository.Interface;
using X1APServer.Repository.Utility;

namespace X1APServer.Repository
{
    public class ETemplateEQuestRepository : X1APBasicRepository<ExportTemplateExtraQuest>, IETemplateEQuestRepository
    {
        public ETemplateEQuestRepository(X1APEntities dbContext) : base(dbContext)
        {
        }

        public IQueryable<ExportTemplateExtraQuest> GetETemplateEQuestList(int templateID)
        {
            var filterQuestList = GetAll().Where(x => x.ExportTemplateID == templateID);
            return filterQuestList;
        }
    }
}
