using Microsoft.VisualStudio.TestTools.UnitTesting;
using X1APServer.Service.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Utils.Tests
{
    [TestClass()]
    public class ROCTests
    {
        [TestMethod()]
        public void TryParseTest()
        {
            var dateStr = "84.12.20";
            var except = new DateTime(1995, 12, 20);
            DateTime output;

            ROC.TryParse(dateStr, out output);

            Assert.AreEqual(except, output);
        }
    }
}