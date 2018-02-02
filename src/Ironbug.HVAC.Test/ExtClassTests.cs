using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ironbug.HVAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ironbug.HVAC.Tests
{
    [TestClass()]
    public class ExtClassTests
    {
        [TestMethod()]
        public void PlantLoops_CloneTo_Test()
        {
            string sFile = @"..\..\..\..\doc\osmFile\pkVAV_HW.osm";
            string tFile = @"..\..\..\..\doc\osmFile\empty.osm";

            string saveFile = @"..\..\..\..\doc\osmFile\empty_.osm";

            var sModel = OpenStudio.Model.load(new OpenStudio.Path(sFile)).get();
            var tModel = OpenStudio.Model.load(new OpenStudio.Path(tFile)).get();

            var loops = sModel.getPlantLoops();
            if (loops.Any())
            {
                foreach (var loop in loops)
                {
                    loop.CloneTo(tModel);
                }
            }

            var success = tModel.Save(saveFile);

            Assert.AreEqual(success, true);
        }

        [TestMethod()]
        public void AirLoop_CloneTo_Test()
        {
            //string sFile = @"..\..\..\..\doc\osmFile\pkVAV_HW.osm";
            string sFile = @"..\..\..\..\doc\osmFile\Sys_7.osm";
            string tFile = @"..\..\..\..\doc\osmFile\empty.osm";

            string saveFile = @"..\..\..\..\doc\osmFile\empty_Added_.osm";

            var sModel = OpenStudio.Model.load(new OpenStudio.Path(sFile)).get();
            var tModel = OpenStudio.Model.load(new OpenStudio.Path(tFile)).get();

            var loops = sModel.getAirLoopHVACs();
            if (loops.Any())
            {
                foreach (var loop in loops)
                {
                    loop.CloneTo(tModel);
                }
            }

            var success = tModel.Save(saveFile);

            Assert.AreEqual(success, true);
        }
    }

    
}
