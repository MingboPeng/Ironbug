using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Ironbug.HVACTests.Loop
{
    [TestClass]
    public class IB_FanConstantVolume_Test
    {
        [TestMethod]
        public void IB_FanConstantVolume_Initialize_Test()
        {
            var fan = new HVAC.IB_FanConstantVolume();
            var dataFields = fan.GetDataFields();
            var success = dataFields.First().StartsWith("Fan Constant Volume 1");
            success = success && dataFields.Count() ==7;

            var sums = fan.ToString();

            Assert.IsTrue(success);
        }
    }
}
