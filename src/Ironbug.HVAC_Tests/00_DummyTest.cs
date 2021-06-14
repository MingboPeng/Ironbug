using System;
using System.Reflection;
using Ironbug.HVAC;
using System.Linq;
using System.Collections.Generic;
using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System.IO;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Ironbug.HVACTests
{
    public class DummyTest
    {
        [Test]
        public void DummyTest_OS_Coil_Heating_WaterFields()
        {
            var tp = typeof(OpenStudio.OS_Coil_Heating_WaterFields);
            var meths = tp.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            var membs = tp.GetMembers(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            Assert.True(true);
        }

        [Test]
        public void IB_TypeTest()
        {
            var coil = new IB_CoilCoolingDXMultiSpeed();
            var success = typeof(IB_Coil).IsInstanceOfType(coil);
            Assert.True(success);
        }

        [Test]
        public void CoolingPlantLoop_Test()
        {
            var lp = new IB_PlantLoop();
            var sz = new IB_SizingPlant();
            sz.SetFieldValue(IB_SizingPlant_FieldSet.Value.LoopType, "Cooling");
            lp.SetSizingPlant(sz);


            var md1 = new Model();
            lp.ToOS(md1);
            string saveFile = TestHelper.GenFileName;
            var success = md1.Save(saveFile);
            Assert.True(success);

            success &= md1.getPlantLoops().First().sizingPlant().loopType() == "Cooling";
            Assert.True(success);
        }

        [Test]
        public void ThermalZoneAndSizingZone_Test()
        {
            var md1 = new OpenStudio.Model();

            var airflow = new OpenStudio.AirLoopHVAC(md1);


            var zone = new OpenStudio.ThermalZone(md1);

            airflow.addBranchForZone(zone);

            
            string saveFile = $"{Guid.NewGuid().ToString().Substring(0,5)}_.osm";
            var success = md1.Save(saveFile);
            

            Assert.True(success);
        }

        [Test]
        public void RetrunIfInModel_Test()
        {
            var md1 = new OpenStudio.Model();

            var airflow = new OpenStudio.AirLoopHVAC(md1);

            var optional = airflow.GetIfInModel(md1);
            
            Assert.True(!(optional is null));
        }

        [Test]
        public void GetEPDoc_Test()
        {
            var note = Ironbug.EPDoc.CoilCoolingWater.Note;
            Assert.True(!string.IsNullOrEmpty(note));

            string temp = Ironbug.EPDoc.CoilCoolingWater.Field_DesignInletWaterTemperature;
            Assert.True(!string.IsNullOrEmpty(temp));
        }

        [Test]
        public void ReadJson()
        {

            //var dir = @"C:\Users\mingo\Documents\GitHub\EPDoc2Json\Doc\";
            //var files = Directory.GetFiles(dir, "*.json");
            //var arr = new List<object>();
            //foreach (var f in files)
            //{

            //    dynamic docObj = JsonConvert.DeserializeObject(File.ReadAllText(f));
            //    var ar = docObj.subsection;
            //    var items = ar.Children();
            //    arr.AddRange(items);
            //}

            ////var list = arr.Children().ToList();

            //Assert.True(true);

        }

        [Test]
        public void OverrideGUIDTest()
        {

            var m = new OpenStudio.Model();
            var p = new OpenStudio.CoilHeatingWater(m);
            p.setName("My Coil");
            var id = p.getString(0).get();

            var newID = $"{{{System.Guid.NewGuid()}}}";
            p.setString(0, newID);

            // override original uuid
            var objID = p.getString(0).get();
            Assert.IsTrue(objID != id);
            Assert.IsTrue(objID == newID);

            // get saving osm and read it back
            var root = System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location);
            var testOsm = System.IO.Path.Combine(root, "test.osm");
            m.Save(testOsm);

            var m2 = OpenStudio.Model.load(testOsm.ToPath()).get();
            var uuid = OpenStudioUtilitiesCore.toUUID(newID);
            var found = m2.getCoilHeatingWater(uuid).get();
            Assert.IsTrue(found != null);
            Assert.IsTrue(found.nameString() == "My Coil");


        }


        [Test]
        public void PlantComponentUserDefined()
        {

            var m = new OpenStudio.Model();
            var p = new OpenStudio.PlantComponentUserDefined(m);


            var dummy = new Node(m);
            var act = new EnergyManagementSystemActuator(dummy, "Plant Connection 1", "Minimum Loading Capacity");
            act.setName("new actuator");
            p.setMinimumLoadingCapacityActuator(act);

            var pManager = new EnergyManagementSystemProgramCallingManager(m);
            pManager.setName("new program manager");
            var program = new EnergyManagementSystemProgram(m);
            program.setName("new program");
            pManager.addProgram(program);

            p.setPlantInitializationProgramCallingManager(pManager);



            // get saving osm and read it back
            var root = System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location);
            var testOsm = System.IO.Path.Combine(root, "test.osm");
            m.Save(testOsm);

            //var m2 = OpenStudio.Model.load(testOsm.ToPath()).get();
            //var uuid = OpenStudioUtilitiesCore.toUUID(newID);
            //var found = m2.getCoilHeatingWater(uuid).get();
            //Assert.IsTrue(found != null);
            //Assert.IsTrue(found.nameString() == "My Coil");


        }

        [Test]
        public void EmsActuators()
        {

            var m = new OpenStudio.Model();
            var p = new OpenStudio.PlantComponentUserDefined(m);

            var actuators = p.emsActuatorNames().Select(_ => $"{_.controlTypeName()}_{_.componentTypeName()}");
            var internalVariables = p.emsInternalVariableNames().Select(_ => _.ToString());

            var l = new PlantLoop(m);
            var lAs = l.emsActuatorNames().Select(_ => $"{_.controlTypeName()}_{_.componentTypeName()}");
            var livs = l.emsInternalVariableNames();
            var lSensors = l.outputVariableNames();

            var pump = new PumpVariableSpeed(m);
            var pAs = pump.emsActuatorNames().Select(_ => $"{_.controlTypeName()}_{_.componentTypeName()}");
            var pivs = pump.emsInternalVariableNames();
            var pSensors = pump.outputVariableNames();
        }

    }
}
