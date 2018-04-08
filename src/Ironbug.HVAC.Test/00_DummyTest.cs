using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ironbug.HVAC;
using System.Linq;
using System.Collections.Generic;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

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
            
            Assert.IsTrue(success);
        }

        

        //[TestMethod]
        //public void GetTypeByName_Test()
        //{

        //    var types = typeof(IB_DataFieldSet).Assembly.GetTypes();
        //    var FieldSetType = typeof(IB_DataFieldSet).Assembly.GetType("Ironbug.HVAC.IB_PumpConstantSpeed_DataFields");

        //    var success = FieldSetType != null;

        //    Assert.IsTrue(success);

        //}


        [TestMethod]
        public void UnderstandBranches_Test()
        {
            var md1 = new OpenStudio.Model();

            var plantloop = new OpenStudio.PlantLoop(md1);

            var boiler1 = new OpenStudio.BoilerHotWater(md1);
            var boiler2 = new OpenStudio.BoilerHotWater(md1);

            boiler1.setName("boiler1");
            boiler2.setName("boiler2");

            var nd = plantloop.supplyInletNode();
            
            
            boiler2.addToNode(nd);
            boiler1.addToNode(nd);


            var components = plantloop.supplyComponents(boiler1.iddObject().type());

            var success = components.First().nameString() == "boiler1";

            string saveFile = @"..\..\..\..\doc\osmFile\empty_Added_.osm";
            success &= md1.Save(saveFile);


            Assert.IsTrue(success);

        }

        [TestMethod]
        public void UnderstandBranches2_Test()
        {
            var md1 = new OpenStudio.Model();

            var plantloop = new OpenStudio.PlantLoop(md1);

            var boiler1 = new OpenStudio.BoilerHotWater(md1);
            var boiler2 = new OpenStudio.BoilerHotWater(md1);
            var boiler3 = new OpenStudio.BoilerHotWater(md1);
            var pump1 = new OpenStudio.PumpConstantSpeed(md1);
            var pump2 = new OpenStudio.PumpVariableSpeed(md1);


            boiler1.setName("boiler1");
            boiler2.setName("boiler2");
            boiler3.setName("boiler3");


            plantloop.addSupplyBranchForComponent(boiler1);
            var node = plantloop.supplyMixer().inletModelObjects().Last().to_Node().get();
            pump1.addToNode(node);

            plantloop.addSupplyBranchForComponent(boiler2);
            node = plantloop.supplyMixer().inletModelObjects().Last().to_Node().get();
            pump2.addToNode(node);

            plantloop.addSupplyBranchForComponent(boiler3);

            //var components = plantloop.supplyComponents(boiler1.iddObject().type());

            //var success = components.First().nameString() == "boiler1";

            string saveFile = @"..\..\..\..\doc\osmFile\empty_Added_.osm";
            var success = md1.Save(saveFile);


            Assert.IsTrue(success);

        }

        

        [TestMethod]
        public void IDDFields_Test()
        {
            var refIddObject = new IdfObject(BoilerHotWater.iddObjectType()).iddObject();
            var iddfield = refIddObject.getField("Design Water Outlet Temperature").get();

            var prettystr = iddfield.getUnits().get().prettyString();
            var standardStr = iddfield.getUnits().get().standardString();
            var str = iddfield.getUnits(true).get().__str__();
            

            //real, choice, alpha,node 
            var dataType = iddfield.properties().type.valueDescription();

            Assert.IsTrue(true);

        }

        [TestMethod]
        public void GetIDDGroup_Test()
        {
            var iddobj = new OpenStudio.IdfObject(OpenStudio.CoilHeatingWater.iddObjectType()).iddObject();
            var gp = iddobj.group();

            Assert.IsTrue(true);

        }

    }
}
