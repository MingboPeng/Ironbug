using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ironbug.Radiance;
using System.IO;

namespace Ironbug.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var inHdr = @"C:\Users\Mingbo\Documents\GitHub\HoneybeeCSharp\doc\testFile\AcceleRad_test_IMG_Perspective_CPU@fc.HDR";
            var outTiff = inHdr.Remove(inHdr.Length - 4)+".tiff";
            var raTiff = new Radiance.Command.RaTiff(inHdr,outTiff);
            raTiff.Execute();

            bool successed = File.Exists(outTiff);
            Assert.AreEqual(successed, true);

        }
    }
}
