using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ironbug.PythonConverter
{
    public static class PyProcessing
    {
        

        public static List<string> GetAllPyFiles()
        {
            string HBCoreFolder = @"C:\Users\Mingbo\Documents\GitHub\Ironbug\LBHB\honeybee";
            List<string> pyFilelist = Directory.GetFileSystemEntries(HBCoreFolder, "*.py").ToList();

            return pyFilelist;
        }

        public static PyClassInfo TranslatePy(string PyFile)
        {
            var PyClass = new PyClassInfo();
            
            var lines = File.ReadAllLines(PyFile, Encoding.UTF8);
            var classNames = new List<string>();
            foreach (var line in lines)
            {
                var cleanLine = line.Trim();
                if (cleanLine.StartsWith("class"))
                {
                    ExtractClassInfo(line, ref PyClass);
                }

                if (cleanLine.StartsWith("def"))
                {
                    ExtractMethodsInfo(line, ref PyClass);
                }

            }
            

            return PyClass;
        }
        public static void ExtractClassInfo(string PyCodeLine, ref PyClassInfo pyClass)
        {

            var names = ExtractElementNames(PyCodeLine)
                            .Where(_=>_!="class").ToArray();
            if (names.Length > 0)
            {
                pyClass.ClassName = names[0];
            }

            if (names.Length > 1)
            {
                pyClass.BaseClassName = names[1];
            }
            
        }

        public static void ExtractMethodsInfo(string PyCodeLine, ref PyClassInfo pyClass)
        {

            var names = ExtractElementNames(PyCodeLine)
                            .Where(_ => (_ != "def")&&(_!="self")).ToArray();
            var method = new PyMethodInfo();
            if (names.Length > 0)
            {
                
                method.Name = names[0];
            }

            if (names.Length > 1)
            {
                method.Inputs = new List<PyValueInfo>() { new PyValueInfo() { ValueType = ValueTypes.Object, Name = names[1] } };
            }

            method.ReturnTypes = ValueTypes.Void;
            

        }

        public static string[] ExtractElementNames(string inputString)
        {
            string input = inputString;

            string regex = @"\W";
            string[] results = Regex.Split(input, regex);
        
            results = results.Where(_ => _ != "").ToArray();
            return results;
        }
        
    }
}
