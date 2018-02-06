using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ironbug.HVACTests.Loop
{
    [TestClass]
    public class IB_CoilHeatingWater_Test
    {
        [TestMethod]
        public void IB_CoilHeatingWater_Initialize_Test()
        {
            var coil = new HVAC.IB_CoilHeatingWater();
            var dataFields = coil.GetDataFields();
            Assert.IsTrue(dataFields.Count() ==10);
        }

        [TestMethod]
        public void IB_CoilHeatingWater_SetAttris_Test()
        {
            var coil = new HVAC.IB_CoilHeatingWater();
            var testValue = 50.0;
            coil.SetAttribute(HVAC.IB_CoilHeatingWater_Attributes.RatedInletWaterTemperature, testValue);
            var att = (double)coil.GetAttributeValue("ratedInletWaterTemperature");

            Assert.IsTrue(att == testValue);
        }


    }
}
