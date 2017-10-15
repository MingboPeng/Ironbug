using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Ironbug.PythonConverter
{
    public enum PyCodeBlockType
    {
        Top,
        Class,
        PropertyGetter,
        PropertySetter,
        Method,

    }

    public class PyCodeBlock
    {
        public string HeaderLine;
        public PyCodeBlockType Type;
        public List<string> CodeBlock;
        public List<PyCodeBlock> ChildBlock;
        public PyCodeBlock()
        {
            this.CodeBlock = new List<string>();
        }

        public PyClassInfo ToPyClassInfo()
        {
            var pyClass = new PyClassInfo();

            //Extract Class Headerline for class name
            ExtractClassInfo(ref pyClass, this.HeaderLine);

            //Extract Method Headerline for its name, input valuables, return types
            if (this.ChildBlock!=null)
            {
                foreach (var item in this.ChildBlock)
                {
                    var methodBlock = item.CodeBlock;
                    if (!methodBlock.First().Contains("@"))
                    {
                        ExtractMethodsInfo(ref pyClass, methodBlock.First(), methodBlock.Last());
                    }
                    
                }
                
            }
            return pyClass;
        }

        public static void ExtractClassInfo(ref PyClassInfo pyClass, string PyCodeLine)
        {

            var names = ExtractElementNames(PyCodeLine);
            if (names.Length > 0)
            {
                pyClass.ClassName = names[0];
            }

            if (names.Length > 1)
            {
                pyClass.BaseClassName = names[1];
            }

        }

        /// <summary>
        /// Extract method name, inputNames, inputTypes, returnType from plain python method code.
        /// </summary>
        /// <param name="pyClass"></param>
        /// <param name="MethodHeader"></param>
        /// <param name="ReturnLine"></param>
        public static void ExtractMethodsInfo(ref PyClassInfo pyClass, string MethodHeader, string ReturnLine)
        {

            var names = ExtractElementNames(MethodHeader);
            var method = new PyMethodInfo();
            
            for (int i = 0; i < names.Length; i++)
            {
                if (i ==0)
                {
                    method.Name = names[0];
                }
                else
                {
                    var line = names[i];
                    var input = new PyValueInfo();
                    input.ValueType = ValueTypes.Object;

                    if (line.Contains("="))
                    {
                        var items = line.Split('=');
                        input.Name = items[0];
                        var defaultValue = items[1].Trim();
                        if (defaultValue =="True" || defaultValue == "False")
                        {
                            input.ValueType = ValueTypes.Bool;
                        }

                    }
                    else
                    {
                        input.Name = line;
                    }

                    method.Inputs.Add(input);
                }
            }

            if (ReturnLine.Contains("return"))
            {
                method.ReturnTypes = ValueTypes.Object;
            }
            else
            {
                method.ReturnTypes = ValueTypes.Void;
            }
            
            pyClass.Methods.Add(method);


        }
        public static string[] ExtractElementNames(string inputString)
        {
            string input = inputString;

            string regex = @"[,\u0028:\u0029 ]|class|self|def";
            string[] results = Regex.Split(input, regex);

            results = results.Where(_ => _ != "").ToArray();
            return results;
        }
    }
}
