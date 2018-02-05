using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ironbug.HVACTests.Loop
{
    [TestClass]
    public class IB_CoilHeatingWater_Test
    {
        [TestMethod]
        public void IB_CoilHeatingWater_GetDataFields()
        {
            var coil = new HVAC.IB_CoilHeatingWater();
            coil.SetAttribute(HVAC.IB_CoilHeatingWater_Attributes.RatedInletWaterTemperature, 50);
            var dataFields = coil.GetDataFields();

            Assert.IsTrue(dataFields.Count() ==10);
        }
    }
}
