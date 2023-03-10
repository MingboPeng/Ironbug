using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Ironbug.HVAC;
using NUnit.Framework;

namespace Ironbug.HVACTests
{

    public class IntegrationTests
    {
        [Test]
        public void Integration()
        {
            var folder = Path.Combine( TestHelper.TestSourceFolder, "Integration Testing");
            var files = Directory.GetFiles(folder);
            var jsons = files.Where(_ => _.EndsWith(".json"));
            var osm = files.First(_ => _.EndsWith(".osm"));

            foreach (var hvac in jsons)
            {
                var fileName = Path.GetFileNameWithoutExtension(hvac);
                Console.WriteLine($"Testing {fileName}");
                var saveAsOsm = Path.Combine(Path.GetTempPath(), $"{fileName}.osm");
                File.Copy(osm, saveAsOsm, true);
                var done = IB_HVACSystem.SaveHVAC(saveAsOsm, hvac);
                if (!done)
                    Console.WriteLine($"Failed to save {hvac}");
                Assert.True(done);

            }
          
        }
    }
}
