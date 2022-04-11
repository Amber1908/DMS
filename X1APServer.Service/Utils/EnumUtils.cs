using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Utils
{
    public static class EnumUtils
    {
        public static IEnumerable<T> GetValues<T>()
        {
            return System.Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}
