using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.IO;

namespace Ironbug.PythonConverter
{
    public class PythonConverter
    {
        string pyPackage = @"..\..\..\..\LBHB";
        string pyLib = @"C:\Program Files\Rhinoceros 5 (64-bit)\Plug-ins\IronPython\Lib";
        //string pyLib = @"C:\Program Files\McNeel\Rhinoceros 5.0\Plug-ins\IronPython\Lib";

        public void DescribePyModulesInFolder()
        {
            string pyPackageFolder = pyPackage;
            string pyModuleFolder = @"honeybee\radiance\command";

            string saveToMainFolder = @"..\..\..\Ironbug.PythonConverter\Outputs\Json";

            string searchPath = pyPackageFolder + pyModuleFolder;
            var files = Directory.GetFiles(searchPath, "*.py");
            
            foreach (var item in files)
            {
                string file =  Path.GetFileNameWithoutExtension(item);

                if (!file.StartsWith("__"))
                {
                    string from = pyModuleFolder.Replace('\\', '.') + '.' + file;

                    char[] a = file.ToCharArray();
                    a[0] = char.ToUpper(a[0]);
                    string import = new string(a);

                    //Save JSON
                    DescribePyModuleAndSaveAsJson(import, saveToMainFolder);
                }
            }
            
           //return files;
        }

        public IList<dynamic> Describe(string ImportString)
        {
            string importString = ImportString;

            string DescriberPyFile = @"..\..\..\Ironbug.PythonConverter\PyModuleDescriber.py";
            try
            {
                var engine = Python.CreateEngine();
                var sourceLibs = engine.GetSearchPaths();
                sourceLibs.Add(this.pyLib);
                sourceLibs.Add(this.pyPackage);
                engine.SetSearchPaths(sourceLibs);

                var scope = engine.CreateScope();

                ScriptSource source = engine.CreateScriptSourceFromFile(DescriberPyFile);

                source.Execute(scope);
                engine.Execute(importString, scope);
                var cal = scope.GetVariable("jsonobj") as IList<dynamic>;
                Console.WriteLine(cal.Count + " modules have been described.");
                return cal;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
                return new List<dynamic>{"Error: " + ex.ToString() };
                //throw;
            }

        }

        public IList<dynamic> DescribePyModule(string Import, bool DebugMode = false) // describe one module
        {
            string importString = string.Format(@"import {0} as desModule;", Import);
            importString += "jsonobj= [PyModuleDescriber().describeModule(desModule)]";

            return this.Describe(importString);
            
        }

        public IList<dynamic> DescribePackage(string Import)  // the package includes all modules
        {
            string importString = string.Format(@"import {0} as desModule;", Import);
            importString += "jsonobj= PyModuleDescriber().describePackage(desModule)";

            return this.Describe(importString);
        }

        //convert python described json object to CSharp code
        public List<string> ConvertToCSharpCode(IList<dynamic> PyModuleObjects)
        {
            var CSCodes = new List<string>();
            foreach (var pyModule in PyModuleObjects)
            {
                var tt = new PyModuleDescription(pyModule);
                CSCodes.Add(tt.CSCode);
                
            }
            
            return CSCodes; //return cs code
        }


        //import honeybee.radiance.command.raTiff
        public string DescribePyModuleAndSaveAsJson(string Import, string SaveTo)
        {
            var jsonString = DescribePyModule(Import).ToString();

            string saveToFile = SaveTo + '\\' + Import.Replace('.', '\\') + ".json";
            string saveToFolder = Path.GetDirectoryName(saveToFile);
            Directory.CreateDirectory(saveToFolder);

            if (jsonString.StartsWith("Error"))
            {
                saveToFile = saveToFile.Replace(".json", "_ERROR.txt");
            }
            File.WriteAllText(saveToFile, jsonString);
            Console.WriteLine(saveToFile);
            return saveToFile;
        }

        static void Main()
        {
            
        }
    }
}
