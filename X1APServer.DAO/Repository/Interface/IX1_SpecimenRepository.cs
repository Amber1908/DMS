using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Repository.Utility.Interface;

namespace X1APServer.Repository.Interface
{
    public interface IX1_SpecimenRepository : IBasicRepository<X1_Specimen>
    {
        /// <summary>
        /// 取得檢體
        /// </summary>
        /// <param name="ccSpecimenID"></param>
        /// <returns></returns>
        X1_Specimen GetSpecimen(string ccSpecimenID = null);
    }
}
