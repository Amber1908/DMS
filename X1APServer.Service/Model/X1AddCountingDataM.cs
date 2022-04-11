using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class X1AddCountingDataM
    {
        public class X1AddCountingDataReq : REQBase
        {
            /// <summary>
            /// 檢體ID
            /// </summary>
            public int MainID { get; set; }
            public decimal Myeloblast { get; set; }
            public decimal Promyelocyte { get; set; }
            public decimal Myelocyte { get; set; }
            public decimal Metamyelocyte { get; set; }
            public decimal Band { get; set; }
            public decimal Segmented { get; set; }
            public decimal EosinophilicSeries { get; set; }
            public decimal Basophils { get; set; }
            public decimal ProErythroblasts { get; set; }
            public decimal ErythroblastsBasophils { get; set; }
            public decimal PolychromaticErythroblasts { get; set; }
            public decimal OrthochromaticErythroblasts { get; set; }
            public decimal Lymphocytes { get; set; }
            public decimal PlasmaCells { get; set; }
            public decimal Monocytes { get; set; }
            public decimal ReticularCells { get; set; }
            public decimal MastCells { get; set; }
        }

        public class X1AddCountingDataRsp : RSPBase
        {

        }
    }
}
