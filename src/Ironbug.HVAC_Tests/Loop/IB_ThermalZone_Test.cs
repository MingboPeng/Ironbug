using Ironbug.HVAC;
using Ironbug.HVAC.BaseClass;
using OpenStudio;
using Xunit;
using Xunit.Abstractions;

namespace Ironbug.HVACTests.Loop
{
    public class IB_ThermalZone_Test
    {
        ITestOutputHelper output;
        public IB_ThermalZone_Test(ITestOutputHelper output)
        {
            this.output = output;
        }
        [Fact]
        public void OpenStudioDll_Test()
        {

            var m = new Model();
            var v = m.version().str();
            output.WriteLine($"Loaded version: {v}");
            Assert.Equal(v, "2.9.0");
        }

        [Fact]
        public void IB_ThermalZone_Sizing_Test()
        {
            string saveFile = @"..\..\..\..\doc\osmFile\empty_Added_.osm"; 

            var obj = new IB_ThermalZone();
            obj.SetAirTerminal(new HVAC.IB_AirTerminalSingleDuctConstantVolumeNoReheat());

            var model = new OpenStudio.Model();
            var lp = new OpenStudio.AirLoopHVAC(model);
            var added1 = lp.addBranchForZone((OpenStudio.ThermalZone)obj.ToOS(model), obj.AirTerminal.ToOS(model));

            var added2 = model.Save(saveFile);
            var success = added1 && added2;
            Assert.True(success);
        }

    }
}
