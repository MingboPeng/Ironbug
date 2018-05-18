using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ironbug.HVAC;
using Ironbug.HVAC.BaseClass;

namespace Ironbug.HVACTests.Loop
{
    [TestClass]
    public class IB_ThermalZone_Test
    {
        
        [TestMethod]
        public void IB_ThermalZone_Sizing_Test()
        {
            string saveFile = @"..\..\..\..\doc\osmFile\empty_Added_.osm"; 

            var obj = new IB_ThermalZone();
            obj.SetAirTerminal(new IB_AirTerminalSingleDuctUncontrolled());

            var model = new OpenStudio.Model();
            var lp = new OpenStudio.AirLoopHVAC(model);
            var added1 = lp.addBranchForZone((OpenStudio.ThermalZone)obj.ToOS(model), (OpenStudio.HVACComponent)obj.AirTerminal.ToOS(model));

            var added2 = model.Save(saveFile);
            var success = added1 && added2;
            Assert.IsTrue(success);
        }

    }
}
