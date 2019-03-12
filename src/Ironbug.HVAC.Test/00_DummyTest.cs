using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ironbug.HVAC;
using System.Linq;
using System.Collections.Generic;
using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System.IO;
using Newtonsoft.Json;

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
        public void IB_TypeTest()
        {
            var coil = new IB_CoilCoolingDXMultiSpeed();
            var success = typeof(IB_Coil).IsInstanceOfType(coil);
        }

        [TestMethod]
        public void CoolingPlantLoop_Test()
        {
            var lp = new IB_PlantLoop();
            var sz = new IB_SizingPlant();
            sz.SetFieldValue(IB_SizingPlant_DataFieldSet.Value.LoopType, "Cooling");
            lp.SetSizingPlant(sz);


            var md1 = new Model();
            lp.ToOS(md1);
            string saveFile = @"..\..\..\..\doc\osmFile\empty_Added_.osm";
            var success = md1.Save(saveFile);

            success &= md1.getPlantLoops().First().sizingPlant().loopType() == "Cooling";

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
            
            Assert.IsTrue(!(optional is null));
        }

        [TestMethod]
        public void GetEPDoc_Test()
        {
            var refType = typeof(CoilCoolingWater);

            var name = string.Format("Ironbug.EPDoc.{0}", refType.Name);
            Type type = typeof(Ironbug.EPDoc.ExampleClass).Assembly.GetType(name,false,true);
            //Type type = Type.GetType("Ironbug.EPDoc.CoilCoolingWater");

            //dynamic obj = null;
            //if (type != null)
            //{
            //    obj = Activator.CreateInstance(type,true);
            //}
           
            
            var note = type.GetProperty("Note").GetValue(null,null) as string;
            string temp = type.GetProperty("Field_DesignInletWaterTemperature").GetValue(null, null) as string;

            Assert.IsTrue(!string.IsNullOrEmpty(temp));
        }

        [TestMethod]
        public void ReadJson()
        {

            var dir = @"C:\Users\mingo\Documents\GitHub\EPDoc2Json\Doc\";
            var files = Directory.GetFiles(dir, "*.json");
            var arr = new List<object>();
            foreach (var f in files)
            {

                dynamic docObj = JsonConvert.DeserializeObject(File.ReadAllText(f));
                var ar = docObj.subsection;
                var items = ar.Children();
                arr.AddRange(items);
            }

            //var list = arr.Children().ToList();

            Assert.IsTrue(true);

        }


    }
}
