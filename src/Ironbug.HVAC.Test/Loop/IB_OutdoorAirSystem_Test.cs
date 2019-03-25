using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ironbug.HVACTests.Loop
{
    [TestClass]
    public class IB_OutdoorAirSystem_Test
    {
        
        [TestMethod]
        public void IB_OutdoorAirSystem_SetController_Test()
        {
            var model = new OpenStudio.Model();
            var loop = new OpenStudio.AirLoopHVAC(model);

            var obj = new HVAC.IB_OutdoorAirSystem();
            var ctrl = new HVAC.IB_ControllerOutdoorAir();

            var testValue = 0.01;
            ctrl.SetFieldValue(HVAC.IB_ControllerOutdoorAir_FieldSet.Value.MinimumOutdoorAirFlowRate, testValue);
            obj.SetController(ctrl);
            obj.AddToNode(loop.supplyOutletNode());

            var inSysCtrl = model.getAirLoopHVACOutdoorAirSystems().First().getControllerOutdoorAir();
            var att = inSysCtrl.minimumOutdoorAirFlowRate();

            Assert.IsTrue(att.get() == testValue);
        }
    }
}
