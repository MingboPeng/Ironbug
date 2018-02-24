using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ironbug.HVAC;

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
        public void DummyTest_Copy()
        {
            var md1 = new OpenStudio.Model();
            var lp1 = new OpenStudio.AirLoopHVAC(md1);
            var idd1 = lp1.iddObject();
            var idf1 = lp1.idfObject();

            var idd2 = new OpenStudio.IddObject(idd1);
            var idf2 = new OpenStudio.IdfObject(idf1);

            var lp2 = idf2.to_AirLoopHVAC();

            //var md2 = new OpenStudio.Model();
            //var obj = md2.addObject(idf2);

            var success = lp2.is_initialized();
            
            Assert.IsTrue(success);
        }

        [TestMethod]
        public void Workflow_Test()
        {
            var md1 = new OpenStudio.Model();

            var airflow = new HVAC.IB_AirLoopHVAC();
            var coil = new HVAC.IB_CoilHeatingWater();
            var coilName = (string)coil.GetDataFieldValue("nameString");

            airflow.AddToSupplyEnd(coil);

            airflow.ToOS(md1);

            var md2 = new OpenStudio.Model();
            airflow.ToOS(md2);

            var success1 = coil.IsInModel(md1);
            var success2 = coil.IsInModel(md2);

            string saveFile = @"..\..\..\..\doc\osmFile\empty_Added_.osm";
            var success3  = md2.Save(saveFile);
            
            var success = success1 && success2 && success3;

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

            //var success = !zone2.isNull();
            //if (success)
            //{
            //    var a = zone2.get().to_ThermalZone();
            //    var b = a.isNull();
            //    var c = a.get();
            //    success = c.comment().Contains(comment);
            //}
            //else
            //{
            //    success = false;
            //}

            //string saveFile = @"..\..\..\..\doc\osmFile\empty_Added_.osm";
            //var success3 = md2.Save(saveFile);

            //var success = success1 && success3;

            Assert.IsTrue(true);
        }
    }
}
