using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Repository.Interface;
using X1APServer.Repository.Utility;

namespace X1APServer.Repository
{
    public class X1_PatientGroupMapRepository : X1APBasicRepository<X1_PatientGroupMap>, IX1_PatientGroupMapRepository
    {
        public X1_PatientGroupMapRepository(X1APEntities dbContext) : base(dbContext)
        {
        }
    }
}
