using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Repository.Utility.Interface;

namespace X1APServer.Repository.Interface
{
    public interface IX1_PatientGroupRepository : IBasicRepository<X1_PatientGroup>
    {
        /// <summary>
        /// 取得所有未刪除群組
        /// </summary>
        /// <returns></returns>
        IQueryable<X1_PatientGroup> GetAllGroup();
        /// <summary>
        /// 取得群組
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        X1_PatientGroup GetGroup(int id);
    }
}
