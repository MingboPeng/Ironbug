using Ironbug.Honeybee.Radiance.Command;
using System.IO;
using Xunit;

namespace Ironbug.Core.Tests
{

    public  class FalsecolorTest
    {
        Falsecolor falsecolor;
        string inHdr;
        string outHdr;

        public void init()
        {

            if (falsecolor is null)
            {
                this.inHdr = @"..\..\..\..\doc\testFile\AcceleRad_test_IMG_Perspective_CPU.HDR";
                this.outHdr = inHdr.Remove(inHdr.Length - 4) + "_fc.HDR";
                this.falsecolor = new Falsecolor(inHdr, outHdr);
            }

        }

        [Fact]
        public void TestFalsecolor_Execute()
        {
            init();
            falsecolor.ExecuteFromPython();
            bool successed = File.Exists(outHdr);
            if (successed)
            {
                File.Delete(outHdr);
            }

            Assert.True(successed);
        }

    }
}
