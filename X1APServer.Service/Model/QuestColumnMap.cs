using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Repository;

namespace X1APServer.Service.Model
{
    public class QuestColumnMap
    {
        public int QuestId { get; set; }
        public int ColumnNum { get; set; }
        public X1_Report_Question Question { get; set; }
    }
}
