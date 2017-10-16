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

        public static List<PyClassInfo> ConvertToPyClassInfos(string PyFile)
        {
            var lines = File.ReadAllLines(PyFile, Encoding.UTF8);
            var classBlocks = GetClassBlocks(lines);
            var PyClassInfoList = new List<PyClassInfo>();

            foreach (var classBlock in classBlocks)
            {
                var pyClassInfo = ProcessChildBlocks(classBlock).ToPyClassInfo();
                PyClassInfoList.Add(pyClassInfo);
            }
            

            return PyClassInfoList;
        }
        
        public static List<PyCodeBlock> GetClassBlocks(string[] RawLines)
        {
            var lines = RawLines;
            var preCleanedClassBlocks = new List<PyCodeBlock>();
            int classLineOffset = -1;
            

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                var cleanLine = line;
                int lineOffset = cleanLine.Length - cleanLine.TrimStart().Length;
                var isClassDefined = classLineOffset != -1;
                
                if (isClassDefined && (lineOffset > classLineOffset))  //this means lines are under class structure
                {
                    preCleanedClassBlocks.Last().CodeBlock.Add(cleanLine);
                    preCleanedClassBlocks.Last().SpaceOffsetLevels.Add(lineOffset);
                }
                else if (cleanLine.Trim().StartsWith("class")) //Class 
                {
                    var classBlock = new PyCodeBlock();
                    classBlock.Type = PyCodeBlockType.Class;
                    classBlock.SpaceOffsetLevels.Add(lineOffset);
                    classBlock.CodeBlock.Add(cleanLine);

                    preCleanedClassBlocks.Add(classBlock);
                    classLineOffset = line.IndexOf("class");
                    
                }
                else 
                {
                    classLineOffset = -1;  // new class lines or imports
                }
            }

            
            return CleanSpaceOffsetLevels(preCleanedClassBlocks);
            
        }

        public static List<PyCodeBlock> CleanSpaceOffsetLevels(List<PyCodeBlock> PyCodeBlocks)
        {
            foreach (var block in PyCodeBlocks)
            {
                //remove duplicated levels
                block.SpaceOffsetLevels = block.SpaceOffsetLevels.Distinct().ToList();
            }

            return PyCodeBlocks;
        }

        private static PyCodeBlock ProcessChildBlocks(PyCodeBlock CleanedClassBlock)
        {
            //0: class level
            //1：method level
            int methodLevelOffset = CleanedClassBlock.SpaceOffsetLevels[1]; 


            //extract methods and properties 
            var codeStrings = CleanedClassBlock.CodeBlock;

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
            
            return classBlock;
        }
        
        

    }

    

    
}
