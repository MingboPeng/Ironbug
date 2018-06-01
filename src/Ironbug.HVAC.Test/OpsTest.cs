using Ironbug.HVAC;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ironbug.HVACTests
{
    [TestClass]
    public class OpsTest
    {
        [TestMethod]
        public void OS_Fields_Test()
        {

            var f = new OS_HeatExchanger_AirToAir_SensibleAndLatentFields();
            var fValues = OS_HeatExchanger_AirToAir_SensibleAndLatentFields.getValues().asVector();
            var v = f.value();

            var f2 = new HeatExchanger_AirToAir_SensibleAndLatentFields();
            var v2 = f2.value();
            var obj = new HeatExchangerAirToAirSensibleAndLatent(new Model());
            var objList = obj.iddObject().objectLists().asVector();
            var objmemo = obj.iddObject().properties().memo;
            var values = HeatExchanger_AirToAir_SensibleAndLatentFields.getValues().asVector();
            var aa = new HeatExchanger_AirToAir_SensibleAndLatentFields().valueName();
            
            var a = new baseUnitConversionFactor(); 
            
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
        public void OSobjFromIDDObj_Test()
        {
            var refIddObject = new IdfObject(BoilerHotWater.iddObjectType()).iddObject();
            var boiler = new StraightComponent(refIddObject.type(), new Model()).to_BoilerHotWater();


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
