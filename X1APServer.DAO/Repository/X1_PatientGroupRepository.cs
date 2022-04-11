using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Repository.Interface;
using X1APServer.Repository.Utility;

namespace X1APServer.Repository
{
    public class X1_PatientGroupRepository : X1APBasicRepository<X1_PatientGroup>, IX1_PatientGroupRepository
    {
        public X1_PatientGroupRepository(X1APEntities dbContext) : base(dbContext)
        {
        }

        public IQueryable<X1_PatientGroup> GetAllGroup()
        {
            var allGroup = GetAll().Where(x => !x.IsDelete);
            return allGroup;
        }

        public X1_PatientGroup GetGroup(int id)
        {
            var group = GetAll().Where(x => !x.IsDelete);

            group = group.Where(x => x.ID == id);

            return group.FirstOrDefault();
        }
    }
}
