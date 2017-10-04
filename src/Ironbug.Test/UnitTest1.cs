using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ironbug;
using System.IO;
using Ironbug.Utilities;

namespace Ironbug.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void RaTiffTest()
        {
            var inHdr = @"C:\Users\Mingbo\Documents\GitHub\HoneybeeCSharp\doc\testFile\AcceleRad_test_IMG_Perspective_CPU@fc.HDR";
            var outTiff = inHdr.Remove(inHdr.Length - 4)+".tiff";
            var raTiff = new Radiance.Command.RaTiff(inHdr,outTiff);
            raTiff.Execute();

            bool successed = File.Exists(outTiff);
            if (successed)
            {
                File.Delete(outTiff);
            }

            Assert.AreEqual(successed, true);

        }

        [TestMethod]
        public void RunRadDirectly()
        {
            //this is a new way calling ratiff.exe directly, instead of using CMD.
            var ranOutput = RadianceRun.RadRun();
            
        }

        [TestMethod]
        public void RunPython()
        {
            var PyRunOutput = PythonRun.PyRun();

            string output = "7";

            Assert.AreEqual(PyRunOutput, output);

        }

        [TestMethod]
        public void RunIronPythonwithOS()
        {

            var PyRunOutput = HoneybeePlusRun.IronPyImportOSRun();

            string output = "c:foo";

            Assert.AreEqual(PyRunOutput, output);

        }

        [TestMethod]
        public void RunIronPyClass()
        {
            var pyOutput = HoneybeePlusRun.HBRun();

            bool successed = pyOutput.Length > 0;

            Assert.AreEqual(true, successed);

        }
    }
}
