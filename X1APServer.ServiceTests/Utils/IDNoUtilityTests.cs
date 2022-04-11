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
    public class IDNoUtilityTests
    {
        [TestMethod()]
        public void CheckIDNoTest()
        {
            Assert.IsFalse(IDNoUtility.CheckIDNo("B12345678"));
            Assert.IsFalse(IDNoUtility.CheckIDNo("B1234567890"));
            Assert.IsFalse(IDNoUtility.CheckIDNo("1B234567890"));
            Assert.IsFalse(IDNoUtility.CheckIDNo("A123456780"));
            Assert.IsTrue(IDNoUtility.CheckIDNo("A123456789"));
        }
    }
}