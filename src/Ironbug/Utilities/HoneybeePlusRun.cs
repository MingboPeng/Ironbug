using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using IronPython.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;

namespace Ironbug.Utilities
{
    public static class HoneybeePlusRun
    {
        public static string HBRun()
        {

            var inHdr = @"C:\Users\Mingbo\Documents\GitHub\HoneybeeCSharp\doc\testFile\AcceleRad_test_IMG_Perspective_CPU@fc.HDR";
            var outTiff = inHdr.Remove(inHdr.Length - 4) + ".tiff";


            var HBPyClass = @"C:\Users\Mingbo\Documents\GitHub\ladybug-tools\honeybee\honeybee\radiance\command\raTiff.py";
            //var HBPyClass = @"C:\Users\Mingbo\Documents\GitHub\HoneybeeCSharp\doc\testFile\testClass.py";
            //var HBPyClass = @"C:\Users\Mingbo\Documents\GitHub\HoneybeeCSharp\doc\testFile\test.py";

            ScriptEngine engine = Python.CreateEngine();
            var sourceLibs = engine.GetSearchPaths();
            //sourceLibs.Add(@"C:\Users\Mingbo\Documents\GitHub\HoneybeeCSharp\packages\IronPython.StdLib.2.7.7\content\Lib");

            //var a =[@'', @"C:\\WINDOWS\\SYSTEM32\\python27.zip", 'C:\\Python27\\DLLs', 'C:\\Python27\\lib', 
            //    'C:\\Python27\\lib\\plat-win', 'C:\\Python27\\lib\\lib-tk', 'C:\\Python27', 
            //    'C:\\Python27\\lib\\site-packages'];

            //sourceLibs.Add(@"C:\WINDOWS\SYSTEM32\python27.zip");
            //sourceLibs.Add(@"C:\Python27\DLLs");
            //sourceLibs.Add(@"C:\Python27\lib");
            //sourceLibs.Add(@"C:\Python27\lib\plat-win");
            //sourceLibs.Add(@"C:\Python27\lib\lib-tk");
            //sourceLibs.Add(@"C:\Python27\");
            //sourceLibs.Add(@"C:\Python27\lib\site-packages");



            sourceLibs.Add(@"C:\Python27\Lib");
            sourceLibs.Add(@"C:\Users\Mingbo\Documents\GitHub\ladybug-tools\honeybee\honeybee\radiance\command");
            sourceLibs.Add(@"C:\Users\Mingbo\Documents\GitHub\ladybug-tools\honeybee\honeybee");

            engine.SetSearchPaths(sourceLibs);
            ScriptSource source = engine.CreateScriptSourceFromFile(HBPyClass);

            ScriptScope scope = engine.CreateScope();
            source.Execute(scope);

            dynamic RaTiff = scope.GetVariable("RaTiff");
            dynamic calc = RaTiff(inHdr,outTiff);
            string result = calc.toRadString();

            return result;
        }
        public static string IronPyImportOSRun()
        {

            var HBPyClass = @"C:\Users\Mingbo\Documents\GitHub\HoneybeeCSharp\doc\testFile\testClass.py";
            ScriptEngine engine = Python.CreateEngine();

            var sourceLibs = engine.GetSearchPaths();
            sourceLibs.Add(@"C:\Python27\Lib");
            engine.SetSearchPaths(sourceLibs);
            ScriptSource source = engine.CreateScriptSourceFromFile(HBPyClass);

            ScriptScope scope = engine.CreateScope();
            //scope.ImportModule("os");

            source.Execute(scope);
            dynamic Calculator = scope.GetVariable("Calculator");
            dynamic calc = Calculator();
            string result = calc.add();

            return result;
            
            
        }
    }
}
