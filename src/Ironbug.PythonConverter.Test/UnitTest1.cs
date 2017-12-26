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

            var extractedInfo = PyProcessing.ConvertToPyClassInfos(PyFile);
            var clsInfo = extractedInfo[0];
            
            Assert.AreEqual("Falsecolor", clsInfo.ClassName);
        }
        

        [TestMethod()]
        public void PyClassInfo_ExportCSFileTest()
        {

            string PyFile = @"C:\Users\Mingbo\Documents\GitHub\Ironbug\LBHB\honeybee\radiance\command\falsecolor.py";

            var extractedInfo = PyProcessing.ConvertToPyClassInfos(PyFile);
            var clsInfo = extractedInfo[0];

            var csFile = @"C:\Users\Mingbo\Documents\GitHub\Ironbug\doc\testFile\exported.cs";
            clsInfo.ExportCSFile(csFile);
            var success = File.Exists(csFile);
            if (success)
            {
                
            }

            Assert.IsTrue(success);
        }

        [TestMethod()]
        public void PyModuleDescriberTest()
        {

            string PyFile = @"..\..\..\Ironbug.PythonConverter\PyModuleDescriber.py";
            PythonEngine engine = new PythonEngine();
            var obj = engine.ExecuteFromFile(PyFile);
            
            var success = obj is null;
            Assert.IsTrue(!success);
        }
        

    }
}
