using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Ironbug.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //string hdrFile = @"C: \Users\Mingbo\Documents\GitHub\HoneybeeCSharp\doc\testFile\AcceleRad_test_IMG_Perspective_CPU.HDR";
            //string pngFile = @"C:\Users\Mingbo\Documents\GitHub\HoneybeeCSharp\doc\testFile\X_4.png";
            //string gifFile = @"C:\Users\Mingbo\Documents\GitHub\HoneybeeCSharp\doc\testFile\indoor_view.GIF";

            const string hdr1 = @"C:\Users\Mingbo\Documents\GitHub\HoneybeeCSharp\doc\testFile\AcceleRad_test_IMG_Perspective_GPU@fc.HDR";
            //const string hdr2 = @"C:\Users\Mingbo\Documents\GitHub\HoneybeeCSharp\doc\testFile\indoor_view.TIF";

            var tiffFileExists = File.ReadAllBytes(hdr1);
            var read2 = File.ReadAllLines(hdr1);
            var length = tiffFileExists.Length;

            //Assert.AreEqual(tiffFileExists, true);
        }
    }
}
