using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ironbug.PythonConverter.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod()]
        public void TEST_DescribePyModule()
        {
            var dec = new PythonConverter();
            string module = "honeybee.radiance.command.epw2wea";
            var obj = dec.DescribePyModule(Import: module);

            var success = !(obj is null) && (obj[0]["Name"] == module);
            Assert.IsTrue(success);
        }

        [TestMethod()]
        public void TEST_ConvertToCSharpCode()
        {
            string module = "honeybee.radiance.command.epw2wea";
            module = "honeybee.radiance.recipe.parameters";
            module = "honeybee.radiance.recipe.recipedcutil";
            //module = "honeybee.radiance.view";

            var dec = new PythonConverter();
            var obj = dec.DescribePyModule(Import: module);
            string cSharpString = dec.ConvertToCSharpCode(obj).First();

            string saveFilePath = @"..\..\..\Ironbug.PythonConverter\Outputs\CSharp\" + module.Replace('.', '\\') + ".cs";
            string saveToFolder = Path.GetDirectoryName(saveFilePath);
            Directory.CreateDirectory(saveToFolder);
            
            File.WriteAllText(saveFilePath, cSharpString);

            var success = File.Exists(saveFilePath);
            Assert.IsTrue(success);

            
        }
    


        //####################################################################################################
        // 
        //####################################################################################################

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
        public void PyModuleDescriberAndSaveTest()
        {
            string saveFolder = @"..\..\..\Ironbug.PythonConverter\Outputs\Json";

            var dec = new PythonConverter();
            var savedFile = dec.DescribePyModuleAndSaveAsJson(Import: "honeybee.radiance.command.epw2wea",SaveTo:saveFolder);
            
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
            var objs = dec.DescribePackage("honeybee");
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
            var objs = dec.DescribePackage("honeybee");
            string cSharpString = dec.ConvertToCSharpCode(objs).First();
            string saveFilePath = @"..\..\..\Ironbug.PythonConverter\Outputs\CSharp\raBmp.cs";
            File.WriteAllText(saveFilePath, cSharpString);

            var success = File.Exists(saveFilePath);
            Assert.IsTrue(success);
            
        }


    }
}
