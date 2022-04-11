using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Repository.Utility.Interface;

namespace X1APServer.Repository.Interface
{
    public interface IX1_ReportQuestionTypeRepository : IBasicRepository<X1_Report_Question_Type>
    {
        /// <summary>
        /// 取得問題類型
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        X1_Report_Question_Type GetQuestType(string name);
    }
}
