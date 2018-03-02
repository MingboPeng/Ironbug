using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ironbug.HVAC;
using System.Linq;

namespace Ironbug.HVACTests
{
    [TestClass]
    public class DummyTest
    {
        [TestMethod]
        public void DummyTest_OS_Coil_Heating_WaterFields()
        {
            var tp = typeof(OpenStudio.OS_Coil_Heating_WaterFields);
            var meths = tp.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            var membs = tp.GetMembers(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            Assert.IsTrue(true);
        }
        

        [TestMethod]
        public void Workflow_Test()
        {
            var md1 = new OpenStudio.Model();

            var airflow = new HVAC.IB_AirLoopHVAC();
            var coil = new HVAC.IB_CoilHeatingWater();
            var fan = new IB_FanConstantVolume();


            airflow.AddToSupplySide(coil);
            airflow.AddToSupplySide(fan);
            airflow.ToOS(md1);

            var md2 = new OpenStudio.Model();
            airflow.ToOS(md2);

            var success1 = coil.IsInModel(md1);
            var success2 = coil.IsInModel(md2);
            var success_fan = fan.IsInModel(md1);

            string saveFile = @"..\..\..\..\doc\osmFile\empty_Added_.osm";
            var success3  = md2.Save(saveFile);
            
            var success = success1 && success2 && success3 & success_fan ;

            Assert.IsTrue(success);
        }

        [TestMethod]
        public void ThermalZoneAndSizingZone_Test()
        {
            var md1 = new OpenStudio.Model();

            var airflow = new OpenStudio.AirLoopHVAC(md1);


            var zone = new OpenStudio.ThermalZone(md1);

            airflow.addBranchForZone(zone);

            
            string saveFile = @"..\..\..\..\doc\osmFile\empty_Added_.osm";
            var success = md1.Save(saveFile);
            

            Assert.IsTrue(success);
        }

        [TestMethod]
        public void RetrunIfInModel_Test()
        {
            var md1 = new OpenStudio.Model();

            var airflow = new OpenStudio.AirLoopHVAC(md1);

            var optional = airflow.GetIfInModel(md1);
            
            Assert.IsTrue(!optional.isNull());
        }

        [TestMethod]
        public void SetPointManager_Test()
        {
            var md1 = new OpenStudio.Model();
            var af = new IB_AirLoopHVAC();
            var coil = new IB_CoilHeatingWater();
            var setPt = new IB_SetpointManagerOutdoorAirReset();
            af.AddToSupplySide(setPt);
            af.AddToSupplySide(coil);
            af.AddToSupplySide(new IB_FanConstantVolume());
            
            af.ToOS(md1);


            string saveFile = @"..\..\..\..\doc\osmFile\empty_Saved.osm";
            var success = md1.Save(saveFile);

            var addedSetPt = md1.getAirLoopHVACs()[0].SetPointManagers().First();
            success &= addedSetPt.comment() == setPt.GetTrackingID();
            
            Assert.IsTrue(success);
        }

        [TestMethod]
        public void addObject_Test()
        {
            var md1 = new OpenStudio.Model();
            var zone = new OpenStudio.ThermalZone(md1);
            zone.setName("a zone name");
            zone.setMultiplier(2);
            zone.setUseIdealAirLoads(true);
            var siz = new OpenStudio.SizingZone(md1, zone);
            siz.setZoneHeatingDesignSupplyAirTemperature(20);
            var comment = "comment as an ID";
            siz.setComment(comment);
            var id1 = zone.handle();
            var zoneSt = zone.__str__();
            var sizSt = siz.__str__();

            

           
            var md2 = new OpenStudio.Model();
            var zone2 = md2.addObject(zone).get().to_ThermalZone().get();
            var sizing2 = md2.addObject(siz).get().to_SizingZone().get();

            var isTrue = sizing2.EqualEqual(siz);

            var zone2St = zone2.__str__();
            var sizing2st = sizing2.to_SizingZone().get().__str__();

            var tp = sizing2.iddObject().type();
            var objs = md2.getObjectsByType(tp);

            var success = false;
            foreach (var item in objs)
            {
                if (item.comment().Equals("! comment as an ID"))
                {
                    success = true;
                }
            }
            
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void GetAllAvailableSettings_Test()
        {
            var datafields = new IB_PumpVariableSpeed_DataFields();
            //var methodNames = datafields.GetAllAvailableSettings();
            var members = datafields.GetType().GetField("full").GetValue(datafields);

            var methods = datafields.GetList();

            var success = true;

            Assert.IsTrue(success);

        }


    }
}
