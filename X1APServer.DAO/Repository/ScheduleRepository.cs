using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Repository.Interface;
using X1APServer.Repository.Utility;

namespace X1APServer.Repository
{
    public class ScheduleRepository : BasicRepository<Schedule>, IScheduleRepository
    {
        public ScheduleRepository(X1APEntities dbContext) : base(dbContext)
        {
        }

        public override Schedule Get(Expression<Func<Schedule, bool>> predicate)
        {
            return base.GetAll().Where(x => !x.IsDelete).FirstOrDefault(predicate);
        }

        public override IQueryable<Schedule> GetAll()
        {
            return base.GetAll().Where(x => !x.IsDelete);
        }

        public override Schedule GetByEntityPrimaryKey(params object[] keyValues)
        {
            return base.GetByEntityPrimaryKey(keyValues);
        }
    }
}
