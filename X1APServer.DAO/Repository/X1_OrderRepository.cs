using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Repository.Interface;
using X1APServer.Repository.Utility;

namespace X1APServer.Repository
{
    public class X1_OrderRepository : BasicRepository<X1_Order>, IX1_OrderRepository
    {
        public X1_OrderRepository(X1APEntities dbContext) : base(dbContext)
        {
        }

        public X1_Order GetOrder(int? sid = null)
        {
            var filterOrder = GetAll();

            if (sid.HasValue)
            {
                filterOrder = filterOrder.Where(x => x.SID == sid);
            }

            return filterOrder.FirstOrDefault();
        }
    }
}
