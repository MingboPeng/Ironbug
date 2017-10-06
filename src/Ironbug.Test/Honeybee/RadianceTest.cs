using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IrHoneybee = Ironbug.Honeybee;
using System.IO;

namespace Ironbug.Test.Honeybee
{
    [TestClass]
    public class RadianceTest
    {
        [TestMethod]
        public void TestRaTiff()
        {
            var inHdr = @"C:\Users\Mingbo\Documents\GitHub\HoneybeeCSharp\doc\testFile\AcceleRad_test_IMG_Perspective_CPU@fc.HDR";
            var outTiff = inHdr.Remove(inHdr.Length - 4) + ".tiff";

            var raTiff = new IrHoneybee.Radiance.Command._RaTiff(inHdr,outTiff);
            raTiff.Execute();

            bool successed = File.Exists(outTiff);
            if (successed)
            {
                File.Delete(outTiff);
            }

            Assert.AreEqual(successed, true);
        }
    }
}
