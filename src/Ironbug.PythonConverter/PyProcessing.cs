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

        public static object TranslatePy()
        {
            string PyFile = @"C:\Users\Mingbo\Documents\GitHub\Ironbug\LBHB\honeybee\radiance\command\falsecolor.py";
            var lines = File.ReadAllLines(PyFile, Encoding.UTF8);
            var classNames = new List<string>();
            foreach (var line in lines)
            {
                var cleanLine = line.Trim();
                if (cleanLine.StartsWith("class"))
                {
                    string className = ExtractClassInfo(cleanLine);
                    classNames.Add(className);
                }
            }

            var dic = new Dictionary<string, List<string>>();
            dic.Add("ClassNames", classNames);

            return dic;
        }
        public static string ExtractClassInfo(string PyCodeLine)
        {
            string inputString = PyCodeLine;
            string className = string.Empty;
            int st = inputString.IndexOf(" ");
            int end = inputString.LastIndexOf(":");

            className = inputString.Substring(st,end-st);

            if (className.Contains("("))
            {
                end = inputString.LastIndexOf("(");
                className = inputString.Substring(st, end-st);
            }
            
            return className.Trim();
        }

        public static string ExtractBaseClassName(string inputString)
        {
            string input = "123ABCDE456FGHIJKL789MNOPQ012";
            
            string pattern = @"\d\w";
            Regex rgx = new Regex(pattern);
            
            string[] result = rgx.Split(input);
            return "";
        }
        
    }
}
