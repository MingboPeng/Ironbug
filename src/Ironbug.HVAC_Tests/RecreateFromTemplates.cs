using System;
using System.IO;
using System.Linq;
using Ironbug.HVAC;
using NUnit.Framework;

namespace Ironbug.HVACTests
{

    public class RecreateFromTemplates
    {
        [Test]
        public void UnitHeaterDuplicateTest()
        {
            var coil = new IB_CoilHeatingElectric();
            var unitHeater = new IB_ZoneHVACUnitHeater();
            unitHeater.SetHeatingCoil(coil);

            var dupCount = 2;
            var dups = unitHeater.Duplicate(dupCount);
            Assert.AreEqual(dupCount, dups.Count);
            var dupIds = dups.Select(_=>_.GetTrackingID()).Distinct();
            Assert.AreEqual(dupCount, dupIds.Count());


            // ensure each unit heater has their own heating coil
            var dupCoils = dups.Select(_ => _.Children.FirstOrDefault());
            Assert.AreEqual(dupCount, dupCoils.Count());
            var dupCoilIds = dupCoils.Select(_ => _.GetTrackingID()).Distinct();
            Assert.AreEqual(dupCount, dupCoilIds.Count());

        }

        //[Test]
        //public void UnitHeaterHWTest()
        //{
        //    var folder = Path.Combine( TestHelper.TestSourceFolder, "Compiling templates");
        //    //var files = Directory.GetFiles(folder);
        //    //var jsons = files.Where(_ => _.EndsWith(".json"));

        //    //D:\Dev\Ironbug\src\Ironbug.HVAC_Tests\TestSource\Compiling templates
        //    var jsonPath = Path.Combine(folder, "UnitHeaterHW.json");
        //    var sysJson = File.ReadAllText(jsonPath);
        //    var system = IB_HVACSystem.FromJson(sysJson);


        //    //foreach (var hvac in jsons)
        //    //{
        //    //    var fileName = Path.GetFileNameWithoutExtension(hvac);
        //    //    Console.WriteLine($"Testing {fileName}");
        //    //    var saveAsOsm = Path.Combine(Path.GetTempPath(), $"{fileName}.osm");
        //    //    File.Copy(osm, saveAsOsm, true);
        //    //    var done = IB_HVACSystem.SaveHVAC(saveAsOsm, hvac);
        //    //    if (!done)
        //    //        Console.WriteLine($"Failed to save {hvac}");
        //    //    Assert.True(done);

        //    //}
          
        //}
    }
}
