using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

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
            var objs = dec.DescribeAllPyModules("honeybee");
            int successCounts = 0;

            foreach (var item in objs)
            {

                string moduleName = item["Name"];
                var moduleClasses = item["Classes"] as IList<dynamic>;

                if (moduleName.StartsWith("honeybee.radiance.command.raBmp"))
                {
                    if (moduleClasses.Count == 1) //
                    {
                        successCounts++;
                    }
                }
                else if (moduleName.StartsWith("honeybee.radiance.command.rmtxop"))
                {
                    if (moduleClasses.Count == 2) //
                    {
                        successCounts++;
                    }
                }

            }
            var success = successCounts == 2;
            Assert.IsTrue(success);
        }
        [TestMethod]
        public void genT4Test()
        {
            var dec = new PythonConverter();
            var objs = dec.DescribeAllPyModules("honeybee");
            string cSharpString = dec.ConvertToCSharpCode(objs);
            string saveFilePath = @"..\..\..\Ironbug.PythonConverter\Outputs\CSharp\raBmp.cs";
            File.WriteAllText(saveFilePath, cSharpString);

            var success = File.Exists(saveFilePath);
            Assert.IsTrue(success);
            
        }


    }
}
