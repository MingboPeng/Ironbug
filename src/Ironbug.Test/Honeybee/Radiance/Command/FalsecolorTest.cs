using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ironbug.Honeybee.Radiance.Command;
using System.IO;

namespace Ironbug.Test
{
   [TestClass]
   public  class FalsecolorTest
    {
        Falsecolor falsecolor;
        string inHdr;
        string outHdr;

        public void init()
        {

            if (falsecolor is null)
            {
                this.inHdr = @"C:\Users\Mingbo\Documents\GitHub\Ironbug\doc\testFile\AcceleRad_test_IMG_Perspective_CPU.HDR";
                this.outHdr = inHdr.Remove(inHdr.Length - 4) + "_fc.HDR";
                this.falsecolor = new Falsecolor(inHdr, outHdr);
            }

        }

        [TestMethod]
        public void TestFalsecolor_Execute()
        {
            init();
            falsecolor.ExecuteFromPython();
            bool successed = File.Exists(outHdr);
            if (successed)
            {
                File.Delete(outHdr);
            }

            Assert.AreEqual(successed, true);
        }

    }
}
