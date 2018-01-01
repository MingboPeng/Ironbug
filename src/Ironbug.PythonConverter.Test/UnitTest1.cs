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
            var dec = new PythonConverter();
            var obj = dec.DescribePyModule(From:"honeybee.radiance.command.epw2wea",Import: "Epw2wea");
            
            var success = obj is null;
            Assert.IsTrue(!success);
        }

        [TestMethod()]
        public void PyModuleDescriberAndSaveTest()
        {
            string saveFolder = @"..\..\..\Ironbug.PythonConverter\Outputs\Json";

            var dec = new PythonConverter();
            var savedFile = dec.DescribePyModuleAndSaveAsJson(From: "honeybee.radiance.command.epw2wea", Import: "Epw2wea",SaveTo:saveFolder);
            
            var success = File.Exists(savedFile);
            Assert.IsTrue(success);
        }

        [TestMethod()]
        public void DescribePyModulesInFolderTest()
        {
            var dec = new PythonConverter();
            dec.DescribePyModulesInFolder();
            
            var success = true;
            Assert.IsTrue(success);
        }

        [TestMethod()]
        public void DescribeAllPyModulesTest()
        {
            var dec = new PythonConverter();
            var obj = dec.DescribeAllPyModules("honeybee");

            var success = obj is null;
            Assert.IsTrue(!success);
        }


    }
}
