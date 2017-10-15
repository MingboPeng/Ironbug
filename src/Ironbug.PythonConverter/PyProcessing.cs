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
            var classBlocks = new List<string>();
            
            foreach (var line in lines)
            {
                var cleanLine = line.Trim();
                if (cleanLine.StartsWith("class"))
                {
                    ExtractClassInfo(cleanLine, ref PyClass);
                }

                if (cleanLine.StartsWith("def"))
                {
                    ExtractMethodsInfo(cleanLine, ref PyClass);
                }
               
            }
            
            return PyClass;
        }
        
        public static List<PyCodeBlock> PreCleanLines(string[] RawLines)
        {
            var lines = RawLines;
            var preCleanedClassBlocks = new List<PyCodeBlock>();
            int classLineOffset = -1;
            int methodLevelOffset = 0;

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                var cleanLine = line;
                int lineOffset = cleanLine.Length - cleanLine.TrimStart().Length;
                var isClassDefined = classLineOffset != -1;
                
                if (isClassDefined && (lineOffset > classLineOffset))  //this means under class structure
                {
                    methodLevelOffset = lineOffset;
                    preCleanedClassBlocks.Last().CodeBlock.Add(cleanLine);
                }
                else if (cleanLine.Trim().StartsWith("class"))
                {
                    var classBlock = new PyCodeBlock();
                    classBlock.Type = PyCodeBlockType.Class;
                    classBlock.CodeBlock.Add(cleanLine);

                    preCleanedClassBlocks.Add(classBlock);
                    classLineOffset = line.IndexOf("class");
                }
                else
                {
                    classLineOffset = -1;  // new class lines or imports
                }
                
            }
            var classBlocks = new List<PyCodeBlock>();
            //extract methods and properties 
            foreach (var item in preCleanedClassBlocks)
            {
                var codeStrings = item.CodeBlock;

                var currentBlockIsProperty = false;

                var classBlock = new PyCodeBlock();
                var properties = new List<PyCodeBlock>();
                var methods = new List<PyCodeBlock>();

                

                for (int i = 0; i < codeStrings.Count; i++)
                {
                    string line = codeStrings[i];
                    
                    int lineOffset = line.Length - line.TrimStart().Length;

                    var isClassHeader = line.Trim().StartsWith("class");
                    var isPropertyMark = line.Trim().StartsWith("@");
                    var isMethod = line.Trim().StartsWith("def");

                    var hasPropertyMask = i > 0 ? codeStrings[i - 1].Trim().StartsWith("@") : false;
                    var isProperty = hasPropertyMask && isMethod;

                    var isMethodLevel = lineOffset == methodLevelOffset;

                    if (isClassHeader)
                    {
                        classBlock.HeaderLine = line;
                    }
                    else if (isPropertyMark)
                    {
                        //isPropertyMark
                        var probertyBlock = new PyCodeBlock();
                        probertyBlock.CodeBlock.Add(line);

                        if (line.Trim().EndsWith("setter"))
                        {
                            probertyBlock.Type = PyCodeBlockType.PropertySetter;
                        }
                        else
                        {
                            probertyBlock.Type = PyCodeBlockType.PropertyGetter;
                        }
                        
                        properties.Add(probertyBlock);
                        currentBlockIsProperty = true;
                        
                    }
                    else if (isProperty)
                    {
                        //isProperty
                        properties.Last().CodeBlock.Add(line);
                        methodLevelOffset = lineOffset;
                    }
                    else if (isMethod)
                    {
                        //isMethod
                        var methodBlock = new PyCodeBlock();
                        methodBlock.CodeBlock.Add(line);
                        methodBlock.Type = PyCodeBlockType.Method;
                        methods.Add(methodBlock);
                        currentBlockIsProperty = false;

                        methodLevelOffset = lineOffset;
                    }
                    else //isChild
                    {
                        if (lineOffset > methodLevelOffset)
                        {
                            if (currentBlockIsProperty) //PropertyChild
                            {
                                properties.Last().CodeBlock.Add(line);
                            }
                            else  //methodChild
                            {
                                methods.Last().CodeBlock.Add(line);
                            }
                        }
                        else //classChild, like private valuable;
                        {
                            classBlock.CodeBlock.Add(line);
                        }

                    }
                    
                }
                classBlock.ChildBlock = new List<PyCodeBlock>();
                classBlock.ChildBlock.AddRange(properties);
                classBlock.ChildBlock.AddRange(methods);
                classBlocks.Add(classBlock);

            }

            return classBlocks;
            
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

            pyClass.Methods.Add(method);
            

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
