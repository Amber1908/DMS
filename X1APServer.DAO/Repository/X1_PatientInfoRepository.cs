using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Repository.Interface;
using X1APServer.Repository.Utility;
using Z.EntityFramework.Plus;

namespace X1APServer.Repository
{
    public class X1_PatientInfoRepository : X1APBasicRepository<X1_PatientInfo>, IX1_PatientInfoRepository
    {
        public X1_PatientInfoRepository(X1APEntities dbContext) : base(dbContext)
        {
        }

        public IQueryable<X1_PatientInfo> GetAllWithReport()
        {
            return GetAll().Include(p => p.X1_Report_Answer_Main.Select(ram => ram.X1_Report_Answer_Detail));
        }

        public X1_PatientInfo GetPatientInfo(string idno)
        {
            var patientInfo = Get(x => x.IDNo.Equals(idno));
            return patientInfo;
        }
    }
}
