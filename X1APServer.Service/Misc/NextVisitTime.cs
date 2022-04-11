using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Misc
{
    public class NextVisitTime : IComparable<NextVisitTime>
    {
        public string Value { get; set; }

        public int CompareTo(NextVisitTime other)
        {
            DateTime current = DateTime.MinValue, next = DateTime.MinValue;
            DateTime.TryParse(Value, out current);
            DateTime.TryParse(other.Value, out next);

            if (!current.Equals(DateTime.MinValue) && !next.Equals(DateTime.MinValue))
            {
                return current.CompareTo(next);
            }
            else if (!current.Equals(DateTime.MinValue))
            {
                return 1;
            }
            else if (!next.Equals(DateTime.MinValue))
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }
}
