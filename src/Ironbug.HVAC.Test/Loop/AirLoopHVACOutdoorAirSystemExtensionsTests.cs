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
    public class AirLoopHVACOutdoorAirSystemExtensionsTests
    {
        [TestMethod()]
        public void OACloneToTest()
        {
            string sFile = @"..\..\..\..\doc\osmFile\Sys_7.osm";

            var sModel = OpenStudio.Model.load(new OpenStudio.Path(sFile)).get();
            var tModel = new OpenStudio.Model();

            var oa = sModel.getAirLoopHVACOutdoorAirSystems().First();
            var newOA = oa.CloneTo(tModel);

            var oaComs = newOA.oaComponents();


            Assert.IsTrue(oaComs.Count > 1);
        }
    }
}