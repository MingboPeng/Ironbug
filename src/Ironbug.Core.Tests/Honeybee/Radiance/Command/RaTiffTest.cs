using System;
using System.IO;
using Ironbug.Honeybee.Radiance.Command;
using Xunit;

namespace Ironbug.Core.Test
{
    public class RaTiffTest
    {
        RaTiff raTiff;
        string inHdr;
        string outTiff;

        public void init()
        {
            
            if (raTiff is null)
            {
                this.inHdr = @"..\..\..\..\doc\testFile\AcceleRad_test_IMG_Perspective_CPU@fc.HDR";
                this.outTiff = inHdr.Remove(inHdr.Length - 4) + ".tiff";
                this.raTiff = new RaTiff(inHdr, outTiff);
            }
            
        }

        [Fact]
        public void TestRaTiff_Execute()
        {
            init();
            raTiff.Execute();
            bool successed = File.Exists(outTiff);
            if (successed)
            {
                File.Delete(outTiff);
            }

            Assert.True(successed);
        }

        [Fact]
        public void TestRaTiff_ExecuteFromPython()
        {
            init();
            raTiff.ExecuteFromPython();
            bool successed = File.Exists(outTiff);
            if (successed)
            {
                File.Delete(outTiff);
            }

            Assert.True(successed);
        }

        [Fact]
        public void TestRaTiff_ToRadString()
        {
            init();

            string radSting = raTiff.ToRadString();
            bool successed = radSting.Contains(this.inHdr);

            Console.WriteLine(radSting);
            Assert.True(successed);
        }


        [Fact]
        public void TestRaTiff_InputFiles_Property()
        {
            init();
            string inputFiles = raTiff.InputFiles.ToString();
            
            bool successed = inputFiles.Contains(this.inHdr);

            Console.WriteLine("inputFiles: " + inputFiles);
            Assert.True(successed);
        }
    }
}
