using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ironbug.PythonConverter;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ironbug.PythonConverter.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod()]
        public void ExtractClassInfoTest()
        {

            string PyFile = @"C:\Users\Mingbo\Documents\GitHub\Ironbug\LBHB\honeybee\radiance\command\falsecolor.py";

            var extractedInfo = PyProcessing.TranslatePy(PyFile);
            var clsInfo = extractedInfo;
            
            Assert.AreEqual("Falsecolor", clsInfo.ClassName);
        }

        [TestMethod()]
        public void PreCleanLinesTest()
        {

            string PyFile = @"C:\Users\Mingbo\Documents\GitHub\Ironbug\LBHB\honeybee\radiance\command\falsecolor.py";

            var lines = File.ReadAllLines(PyFile, Encoding.UTF8);
            var classBlocks = PyProcessing.PreCleanLines(lines);
            var classBlock = classBlocks[0].ToPyClassInfo();

            Assert.AreEqual("Falsecolor", classBlock.ClassName);
        }
    }
}
