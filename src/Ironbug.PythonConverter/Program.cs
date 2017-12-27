using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System;
using System.IO;

namespace Ironbug.PythonConverter
{
    public class PythonConverter
    {
        public void DescribePyModulesInFolder()
        {
            string pyPackageFolder = @"C:\Users\mpeng\AppData\Roaming\McNeel\Rhinoceros\5.0\scripts\";
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

        public object DescribePyModule(string From, string Import)
        {
            string importString = string.Format(@"from {0} import {1} as desModule;", From, Import);
            importString += "jsonobj= PyModuleDescriber().describe(desModule)";
            
            string DescriberPyFile = @"..\..\..\Ironbug.PythonConverter\PyModuleDescriber.py";
            try
            {
                var engine = Python.CreateEngine();
                var sourceLibs = engine.GetSearchPaths();
                sourceLibs.Add(@"C:\Program Files\McNeel\Rhinoceros 5.0\Plug-ins\IronPython\Lib"); 
                sourceLibs.Add(@"C:\Users\mpeng\AppData\Roaming\McNeel\Rhinoceros\5.0\scripts");
                engine.SetSearchPaths(sourceLibs);

                var scope = engine.CreateScope();

                ScriptSource source = engine.CreateScriptSourceFromFile(DescriberPyFile);

                source.Execute(scope);
                engine.Execute(importString, scope);
                var cal = scope.GetVariable("jsonobj");
                Console.WriteLine(cal);
                return cal;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return "Error: " + ex.ToString();
                //throw;
            }
            
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
