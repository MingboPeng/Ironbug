using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FreeImageAPI;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Ironbug.test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string hdrFile = @"C: \Users\Mingbo\Documents\GitHub\HoneybeeCSharp\doc\testFile\AcceleRad_test_IMG_Perspective_CPU.HDR";
            string pngFile = @"C:\Users\Mingbo\Documents\GitHub\HoneybeeCSharp\doc\testFile\X_4.png"; 
            string gifFile = @"C:\Users\Mingbo\Documents\GitHub\HoneybeeCSharp\doc\testFile\indoor_view.GIF";
            
            //var img = FreeImage.Load(FREE_IMAGE_FORMAT.FIF_HDR, hdrFile, FREE_IMAGE_LOAD_FLAGS.DEFAULT);
            ////FreeImage.to

            //var img2 = FreeImageBitmap.FromFile(hdrFile);
            
            //var bImg = img2.ToBitmap();
            //var size= bImg.Size;
            
            //var imgString = size.ToString();

            // For the example
            const string ex1 = "ra_tiff indoor_view_h.HDR indoor_view.TIF";
            const string hdr1 = @"C:\Users\Mingbo\Documents\GitHub\HoneybeeCSharp\doc\testFile\indoor_view_h.HDR";
            const string hdr2 = @"C:\Users\Mingbo\Documents\GitHub\HoneybeeCSharp\doc\testFile\indoor_view.TIF";
            
            
            //var cmdStrings = new List<string>();
            //cmdStrings.Add(@"C:\Users\Mingbo\Documents\GitHub\HoneybeeCSharp\Ironbug.test\bin\Debug\");
            //cmdStrings.Add(@"ra_tiff indoor_view_h.HDR indoor_viewa.TIF");
            //CMD.Execute(cmdStrings);

            var filePath = hdr1;
            string tiffFile = string.Empty;

            if (File.Exists(filePath) && Path.GetExtension(filePath).ToUpper() == ".HDR")
            {
                tiffFile =filePath.Substring(0,filePath.Length - 3) + "TIF";
                string cmdStr1 = @"ra_tiff " + filePath + " " + tiffFile;
                var cmdStrings = new List<string>();
                cmdStrings.Add(@"SET RAYPATH=.;C:\Radiance\lib&PATH=C:\Radiance\bin;$PATH");
                cmdStrings.Add(cmdStr1);
                CMD.Execute(cmdStrings);

            }

            bool tiffFileExists = File.Exists(tiffFile);

            Assert.AreEqual(tiffFileExists, true);
            //byte[] fileBytes = File.ReadAllBytes(hdr1);
            //var files = File.ReadAllText(hdr1);
            //StringBuilder sb = new StringBuilder();

            //foreach (byte b in fileBytes)
            //{
            //    sb.Append(Convert.ToString(b));
            //    //sb.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
            //}

            //File.WriteAllText(outputFilename, sb.ToString());


        }
    }
}
