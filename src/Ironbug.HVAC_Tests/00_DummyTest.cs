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
            string saveFile = @"..\..\..\..\doc\osmFile\empty_Added_.osm";
            var success = md1.Save(saveFile);

            success &= md1.getPlantLoops().First().sizingPlant().loopType() == "Cooling";

            Assert.True(true);
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


    }
}
