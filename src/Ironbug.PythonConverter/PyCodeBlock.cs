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
        Constructor,
        PropertyGetter,
        PropertySetter,
        Method,
        
    }

    public class PyCodeBlock
    {
        public string HeaderLine { get; set; }
        public PyCodeBlockType Type { get; set; }
        public List<string> CodeBlock { get; set; }
        public List<PyCodeBlock> ChildBlock { get; set; }
        public List<int> SpaceOffsetLevels { get; set; }

        public PyCodeBlock()
        {
            this.CodeBlock = new List<string>();
            this.SpaceOffsetLevels = new List<int>();
        }

        /// <summary>
        /// Convert Python code class block to PyClassInfo type
        /// </summary>
        /// <returns>PyClassInfo</returns>
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
                    var currentBlock = item;
                    var code = currentBlock.CodeBlock;
                    var firstLine = code.First();

                    if (firstLine.Contains("_init_"))
                    {
                        ExtractConstractorInfo(ref pyClass, code.First());
                    }
                    else if(currentBlock.Type == PyCodeBlockType.Method)
                    {
                        ExtractMethodsInfo(ref pyClass, code.First(), code.Last());
                    }
                    else if (currentBlock.Type == PyCodeBlockType.PropertyGetter)
                    {
                        ExtractPropertyInfo(ref pyClass, code[1]);
                    }
                    else if (currentBlock.Type == PyCodeBlockType.PropertySetter)
                    {
                        //ignore setter
                    }




                }
                
            }
            return pyClass;
        }

        

        private static void ExtractClassInfo(ref PyClassInfo pyClass, string ClassHeader)
        {
            var elements = ExtractElementNames(ClassHeader);
            pyClass.ClassName = elements[0];
            
            if (elements.Length > 1)
            {
                pyClass.BaseClassName = elements[1];
            }

        }

        private static void ExtractPropertyInfo(ref PyClassInfo pyClass, string PropertyHeader)
        {
            var elements = ExtractElementNames(PropertyHeader);
            var property = new PyPropergyInfo();
            property.Name = elements[0];
            pyClass.Properties.Add(property);

        }

        private void ExtractConstractorInfo(ref PyClassInfo pyClass, string ConstructorHeader)
        {
            var elements = ExtractElementNames(ConstructorHeader);
            var constructor = new PyConstuctorInfo();
            constructor.Name = pyClass.ClassName;
            constructor.Inputs.AddRange(ExtractInputsInfo(elements));

            pyClass.Constuctor = constructor;
        }

        /// <summary>
        /// Extract method name, inputNames, inputTypes, returnType from plain python method code.
        /// </summary>
        /// <param name="pyClass"></param>
        /// <param name="MethodHeader"></param>
        /// <param name="ReturnLine"></param>
        private static void ExtractMethodsInfo(ref PyClassInfo pyClass, string MethodHeader, string ReturnLine)
        {

            var elements = ExtractElementNames(MethodHeader);
            var method = new PyMethodInfo();
            method.Name = elements[0];
            method.Inputs.AddRange(ExtractInputsInfo(elements));
            
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

        private static List<PyValueInfo> ExtractInputsInfo(string[] elements)
        {
            var inputList = new List<PyValueInfo>();
            for (int i = 0; i < elements.Length; i++)
            {
                if (i == 0)
                {
                    //this is the header's name
                }
                else
                {
                    var line = elements[i];
                    var input = new PyValueInfo();
                    input.ValueType = ValueTypes.Object;

                    if (line.Contains("="))
                    {
                        var items = line.Split('=');
                        input.Name = items[0];
                        var defaultValue = items[1].Trim();
                        if (defaultValue == "True" || defaultValue == "False")
                        {
                            input.ValueType = ValueTypes.Bool;
                        }

                    }
                    else
                    {
                        input.Name = line;
                    }

                    inputList.Add(input);
                }
            }


            return inputList;
        }

        private static string[] ExtractElementNames(string inputString)
        {
            string input = inputString;

            string regex = @"[,\u0028:\u0029 ]|class|self|def";
            string[] results = Regex.Split(input, regex);

            results = results.Where(_ => _ != "").ToArray();
            return results;
        }
    }
}
