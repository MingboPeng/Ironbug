using NUnit.Framework;
using System.Linq;

namespace Ironbug.HVACTests
{
    public class IB_OutdoorAirSystem_Test
    {
        [SetUp]
        public void Init()
        {

        }


        [Test]
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

            Assert.True(att.get() == testValue);
        }
    }
}
