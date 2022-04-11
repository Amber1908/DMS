using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Repository.Interface;
using X1APServer.Repository.Utility;

namespace X1APServer.Repository
{
    public class X1_SpecimenRepository : BasicRepository<X1_Specimen>, IX1_SpecimenRepository
    {
        public X1_SpecimenRepository(X1APEntities dbContext) : base(dbContext)
        {
        }

        public X1_Specimen GetSpecimen(string ccSpecimenID = null)
        {
            var allSpecimen = GetAll();

            if (ccSpecimenID != null)
            {
                allSpecimen = allSpecimen.Where(x => x.CCSpecimenID.Equals(ccSpecimenID));
            }

            return allSpecimen.FirstOrDefault();
        }
    }
}
