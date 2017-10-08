using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Ironbug.Core.Honeybee.Radiance.Command;

namespace Ironbug.Test
{
    [TestClass]
    public class RaTiffTest
    {
        RaTiff raTiff;
        string inHdr;
        string outTiff;

        public void initRaTiff()
        {
            
            if (raTiff is null)
            {
                this.inHdr = @"C:\Users\Mingbo\Documents\GitHub\Ironbug\doc\testFile\AcceleRad_test_IMG_Perspective_CPU@fc.HDR";
                this.outTiff = inHdr.Remove(inHdr.Length - 4) + ".tiff";
                this.raTiff = new RaTiff(inHdr, outTiff);
            }
            
        }

        [TestMethod]
        public void TestRaTiff_Execute()
        {
            initRaTiff();
            raTiff.Execute(RunFromPython: false);
            bool successed = File.Exists(outTiff);
            if (successed)
            {
                File.Delete(outTiff);
            }

            Assert.AreEqual(successed, true);
        }

        [TestMethod]
        public void TestRaTiff_ToRadString()
        {
            initRaTiff();

            string radSting = raTiff.ToRadString();
            bool successed = radSting.Contains(this.inHdr);

            Console.WriteLine(radSting);
            Assert.AreEqual(successed, true);
        }


        [TestMethod]
        public void TestRaTiff_RadbinPath_Property()
        {
            initRaTiff();
            var radbinPath = raTiff.RadbinPath;

            bool successed = radbinPath.Contains("radiance");

            Console.WriteLine("radbinPath: " + radbinPath);
            Assert.AreEqual(successed, true);
        }
    }
}
