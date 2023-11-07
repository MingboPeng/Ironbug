using Ironbug.HVAC;
using Ironbug.HVAC.BaseClass;
using OpenStudio;
using NUnit.Framework;
using System;

namespace Ironbug.HVACTests
{
    public class IB_ThermalZone_Test
    {
        private string GenFileName => TestHelper.GenFileName;
        [Test]
        public void OpenStudioDll_Test()
        {

            var m = new Model();
            var v = m.version().str();
            Console.WriteLine($"Loaded version: {v}");
            Assert.IsTrue(v.StartsWith("3.6.1"));
        }

        [Test]
        public void OldOsmFile_Test()
        {
            var oldFile = System.IO.Path.Combine(TestHelper.TestSourceFolder, "OldVersion.osm");
            var oldModel = IB_Utility.GetOrNewModel(oldFile);
            var version = oldModel.version().str(); // updated to the current version

            //var ex = Assert.Throws<ArgumentException>(() => IB_HVACSystem.GetOrNewModel(oldFile));
            //Assert.IsTrue(ex.Message.StartsWith("Incompatible input OpenStudio file version"));
            Assert.IsTrue(!version.StartsWith("2.4"));
        }


        [Test]
        public void IB_ThermalZone_Sizing_Test()
        {
            string saveFile = GenFileName;

            var obj = new IB_ThermalZone();
            obj.SetAirTerminal(new HVAC.IB_AirTerminalSingleDuctConstantVolumeNoReheat());

            var model = new OpenStudio.Model();
            var lp = new OpenStudio.AirLoopHVAC(model);
            var added1 = lp.addBranchForZone((OpenStudio.ThermalZone)obj.ToOS(model), obj.AirTerminal.ToOS(model));
            Assert.IsTrue(added1);

            var added2 = model.Save(saveFile);
            Assert.IsTrue(added2);

        }

    }
}
