using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ironbug.HVAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ironbug.HVAC.Tests
{
    //[TestClass()]
    public class ExtClassTests
    {
        //[TestMethod()]
        //public void PlantLoops_CloneTo_Test()
        //{
        //    string sFile = @"..\..\..\..\doc\osmFile\pkVAV_HW.osm";
        //    string tFile = @"..\..\..\..\doc\osmFile\empty.osm";

        //    string saveFile = @"..\..\..\..\doc\osmFile\empty_.osm";

        //    var sModel = OpenStudio.Model.load(OpenStudio.OpenStudioUtilitiesCore.toPath(sFile)).get();
        //    var tModel = OpenStudio.Model.load(OpenStudio.OpenStudioUtilitiesCore.toPath(tFile)).get();

        //    var loops = sModel.getPlantLoops();
        //    if (loops.Any())
        //    {
        //        foreach (var loop in loops)
        //        {
        //            loop.CloneTo(tModel);
        //        }
        //    }

        //    var success = tModel.Save(saveFile);

        //    Assert.AreEqual(success, true);
        //}

        //[TestMethod()]
        //public void AirLoop_CloneTo_Test()
        //{
        //    //string sFile = @"..\..\..\..\doc\osmFile\pkVAV_HW.osm";
        //    string sFile = @"..\..\..\..\doc\osmFile\Sys_7.osm";
        //    string tFile = @"..\..\..\..\doc\osmFile\empty.osm";

        //    string saveFile = @"..\..\..\..\doc\osmFile\empty_Added_.osm";

        //    var sModel = OpenStudio.Model.load(OpenStudio.OpenStudioUtilitiesCore.toPath(sFile)).get();
        //    var tModel = OpenStudio.Model.load(OpenStudio.OpenStudioUtilitiesCore.toPath(tFile)).get();

        //    var loops = sModel.getAirLoopHVACs();
        //    if (loops.Any())
        //    {
        //        foreach (var loop in loops)
        //        {
        //            loop.CloneTo(tModel);
        //        }
        //    }
            

        //    var success = tModel.Save(saveFile);

        //    Assert.AreEqual(success, true);
        //}
        [TestMethod()]
        public void AirLoop_Component_Test()
        {
            //string sFile = @"..\..\..\..\doc\osmFile\pkVAV_HW.osm";
            string sFile = @"..\..\..\..\doc\osmFile\Sys_7.osm";
            string tFile = @"..\..\..\..\doc\osmFile\empty.osm";

            string saveFile = @"..\..\..\..\doc\osmFile\empty_Added_.osm";

            var sModel = OpenStudio.Model.load(OpenStudio.OpenStudioUtilitiesCore.toPath(sFile)).get();
            var tModel = OpenStudio.Model.load(OpenStudio.OpenStudioUtilitiesCore.toPath(tFile)).get();

            var loops = sModel.getAirLoopHVACs();
            if (loops.Any())
            {
                foreach (var loop in loops)
                {
                    var com = loop.createComponent();
                    tModel.insertComponent(com);
                }
            }


            var success = tModel.Save(saveFile);

            Assert.AreEqual(success, true);
        }


        [TestMethod()]
        public void ThermalZone_CloneTo_Test()
        {
            //string sFile = @"..\..\..\..\doc\osmFile\pkVAV_HW.osm";
            string sFile = @"..\..\..\..\doc\osmFile\Sys_7.osm";
            //string tFile = @"..\..\..\..\doc\osmFile\empty.osm";

            string saveFile = @"..\..\..\..\doc\osmFile\empty_Added_.osm";

            var sModel = OpenStudio.Model.load(OpenStudio.OpenStudioUtilitiesCore.toPath(sFile)).get();
            //var tModel = OpenStudio.Model.load(OpenStudio.OpenStudioUtilitiesCore.toPath(tFile)).get();

            var loops = sModel.getAirLoopHVACs();

            //var zones = sModel.getThermalZones();
            var zone = new OpenStudio.ThermalZone(sModel);
            var zoneSizing = new OpenStudio.SizingZone(sModel, zone);
            var added = loops.First().addBranchForZone(zone);
            //foreach (var item in zones)
            //{
            //    //var zone = item.clone(tModel).to_ThermalZone().get();
            //    var added = loops.First().addBranchForZone(item);
            //}
            //var addedZones = tModel.getThermalZones();
            var success = loops.First().thermalZones().Count > 0;
            sModel.Save(saveFile);
            Assert.AreEqual(success, true);
        }
    }

    
}
