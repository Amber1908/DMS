using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Repository.Utility.Interface;

namespace X1APServer.Repository.Interface
{
    public interface IX1_OrderRepository : IBasicRepository<X1_Order>
    {
        X1_Order GetOrder(int? sid = null);
    }
}
