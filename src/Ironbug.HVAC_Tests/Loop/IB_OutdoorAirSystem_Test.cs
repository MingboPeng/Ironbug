using Ironbug.HVAC;
using NUnit.Framework;
using System.Collections.Generic;
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
        public void Duplicate_Test()
        {
            var model = new OpenStudio.Model();
            var loop = new OpenStudio.AirLoopHVAC(model);

            var obj = new HVAC.IB_OutdoorAirSystem();
            var ctrl = new HVAC.IB_ControllerOutdoorAir();

            var testValue = 0.01;
            ctrl.SetFieldValue(HVAC.IB_ControllerOutdoorAir_FieldSet.Value.MinimumOutdoorAirFlowRate, testValue);
            obj.SetController(ctrl);
            obj.AddToNode(model, loop.supplyOutletNode());

            var stp = new HVAC.IB_SetpointManagerScheduled(13.6);
            obj.OAStreamObjs.Add(stp);

            var dup = obj.Duplicate() as HVAC.IB_OutdoorAirSystem;
            Assert.AreEqual(obj, dup);
            Assert.AreEqual(obj.OAStreamObjs, dup.OAStreamObjs);

            var json = obj.ToJson(true);
            var dup2 = HVAC.IB_OutdoorAirSystem.FromJson<IB_OutdoorAirSystem>(json);
            Assert.AreEqual(obj, dup2);
            Assert.AreEqual(obj.OAStreamObjs, dup2.OAStreamObjs);
            Assert.AreEqual(obj.OAStreamObjs.First(), dup2.OAStreamObjs.First());

            Assert.AreEqual(json, dup2.ToJson(true));
        }

        [Test]
        public void SetpointManagerScheduled_Test()
        {
            var obj = new HVAC.IB_SetpointManagerScheduled(13.6);

            var dup = obj.Duplicate() as HVAC.IB_SetpointManagerScheduled;
            Assert.AreEqual(obj, dup);

            var json = obj.ToJson(true);
            var dup2 = HVAC.IB_SetpointManagerScheduled.FromJson<IB_SetpointManagerScheduled>(json);
            Assert.AreEqual(obj, dup2);

        }

        [Test]
        public void SetpointManagerScheduledList_Test()
        {
            var obj = new HVAC.IB_SetpointManagerScheduled(13.6);
            var list = new List<HVAC.IB_SetpointManagerScheduled>() { obj };
        

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(list, IB_JsonSetting.ConvertSetting);
            var dup2 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<HVAC.IB_SetpointManagerScheduled>>(json, IB_JsonSetting.ConvertSetting);
      
            Assert.AreEqual(list, dup2);

        }

        [Test]
        public void SetController_Test()
        {
            var model = new OpenStudio.Model();
            var loop = new OpenStudio.AirLoopHVAC(model);

            var obj = new HVAC.IB_OutdoorAirSystem();
            var ctrl = new HVAC.IB_ControllerOutdoorAir();

            var testValue = 0.01;
            ctrl.SetFieldValue(HVAC.IB_ControllerOutdoorAir_FieldSet.Value.MinimumOutdoorAirFlowRate, testValue);
            obj.SetController(ctrl);
            obj.AddToNode(model, loop.supplyOutletNode());

            var inSysCtrl = model.getAirLoopHVACOutdoorAirSystems().First().getControllerOutdoorAir();
            var att = inSysCtrl.minimumOutdoorAirFlowRate();

            Assert.True(att.get() == testValue);

            
        }
    }
}
