using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ironbug.HVAC;

namespace Ironbug.HVACTests.Loop
{
    [TestClass]
    public class IB_ThermalZone_Test
    {
        [TestMethod]
        public void IB_ThermalZone_Initialize_Test()
        {
            var obj = new HVAC.IB_ThermalZone();
            var dataFields = obj.GetDataFields();
            Assert.IsTrue(dataFields.Count() >0);
        }

        [TestMethod]
        public void IB_ThermalZone_Sizing_Test()
        {
            string saveFile = @"..\..\..\..\doc\osmFile\empty_Added_.osm"; 

            var obj = new HVAC.IB_ThermalZone();
            obj.AirTerminal = new HVAC.IB_AirTerminal();

            var model = new OpenStudio.Model();
            var lp = new OpenStudio.AirLoopHVAC(model);
            var added1 = lp.addBranchForZone((OpenStudio.ThermalZone)obj.ToOS(model), (OpenStudio.HVACComponent)obj.AirTerminal.ToOS(model));

            var added2 = model.Save(saveFile);
            var success = added1 && added2;
            Assert.IsTrue(success);
        }

    }
}
