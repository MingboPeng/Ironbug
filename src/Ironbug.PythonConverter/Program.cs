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
                    DescribePyModuleAndSaveAsJson(from, import, saveToMainFolder);
                }
            }
            
           //return files;
        }

        public object DescribePyModule(string ImportString)
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
                Console.WriteLine(cal.Count + " modules described.");
                return cal;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return "Error: " + ex.ToString();
                //throw;
            }

        }

        public object DescribePyModule(string From, string Import)
        {
            string importString = string.Format(@"from {0} import {1} as desModule;", From, Import);
            importString += "jsonobj= PyModuleDescriber().describe(desModule)";

            return this.DescribePyModule(importString);
            
        }

        public object DescribeAllPyModules(string Import)
        {
            string importString = string.Format(@"import {0} as desModule;", Import);
            importString += "jsonobj= PyModuleDescriber().describeAll(desModule)";

            return this.DescribePyModule(importString);
        }

        
        
        public string DescribePyModuleAndSaveAsJson(string From, string Import, string SaveTo)
        {
            var jsonString = DescribePyModule(From,Import).ToString();

            string saveToFile = SaveTo + '\\' + From.Replace('.', '\\') + ".json";
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
