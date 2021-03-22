using Ironbug.HVAC;
using Ironbug.HVAC.BaseClass;
using OpenStudio;
using NUnit.Framework;
using System;

namespace Ironbug.HVACTests
{
    public class IB_ThermalZone_Test
    {
      
        [Test]
        public void OpenStudioDll_Test()
        {

            var m = new Model();
            var v = m.version().str();
            Console.WriteLine($"Loaded version: {v}");
            Assert.IsTrue(v == "3.1.0");
        }

        [Test]
        public void IB_ThermalZone_Sizing_Test()
        {
            string saveFile = @"..\..\..\..\doc\osmFile\empty_Added_.osm"; 

            var obj = new IB_ThermalZone();
            obj.SetAirTerminal(new HVAC.IB_AirTerminalSingleDuctConstantVolumeNoReheat());

            var model = new OpenStudio.Model();
            var lp = new OpenStudio.AirLoopHVAC(model);
            var added1 = lp.addBranchForZone((OpenStudio.ThermalZone)obj.ToOS(model), obj.AirTerminal.ToOS(model));

            var added2 = model.Save(saveFile);
            var success = added1 && added2;
            Assert.True(success);
        }

    }
}
