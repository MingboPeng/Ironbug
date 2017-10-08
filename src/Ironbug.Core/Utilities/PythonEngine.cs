using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Ironbug.Core
{
    public class PythonEngine
    {
        private ScriptEngine _engine;
        private MemoryStream _ms;
        private MemoryStream _er;
        public PythonEngine()
        {
            //create Engine
            this._engine = Python.CreateEngine();

            var sourceLibs = this._engine.GetSearchPaths();
            //sourceLibs.Add(@"C:\Python27\Lib"); //local python installed 
            sourceLibs.Add(@"C:\Program Files\Rhinoceros 5 (64-bit)\Plug-ins\IronPython\Lib"); //from Rhino
            //sourceLibs.Add(@"C:\Program Files\Rhinoceros 5 (64-bit)\Plug-ins\IronPython\Lib"); //from Dynamo ???

            sourceLibs.Add(@"C:\Users\Mingbo\Documents\GitHub\Ironbug\LBHB"); //LadybugPlus HoneybeePlus core libriary
            this._engine.SetSearchPaths(sourceLibs);
            
            ScriptScope ClrModule = _engine.GetClrModule();
            string[] moduleNames = _engine.GetModuleFilenames();


            _ms = new MemoryStream();
            _er = new MemoryStream();
            _engine.Runtime.IO.SetOutput(_ms, Encoding.UTF8);
            _engine.Runtime.IO.SetErrorOutput(_er, Encoding.UTF8);
        }

        private object GetPyModule(string ImportStrings, string ModuleName)
        {
            string importStrings = ImportStrings;
            string pyModuleName = ModuleName;
            try
            {
                //import HoneybeePlus module
                var pyImportString = "import sys;sys.platform = 'win32';";
                pyImportString += importStrings;
                //ScriptSource source = this._engine.CreateScriptSourceFromString(pyImportString);
                ScriptScope scope = this._engine.CreateScope();
                
                this._engine.Execute(pyImportString, scope);

                //var code = source.Compile();

                string outputStrings = ReadStream(_ms);
                string errorStrings = ReadStream(_er);
                dynamic obj = scope.GetVariable(pyModuleName);
                return obj;
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.ToString());
                //var outputStreamWriter = new StreamWriter(_ms);
                //outputStreamWriter.Write(ex.ToString());
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public object Import(string ClassName)
        {
            var pyImportString = string.Format(@"import {0};", ClassName);
            object obj = GetPyModule(pyImportString, ClassName);
            return obj;
        }

        public object ImportFrom(string From, string Import)
        {
            var pyImportString = string.Format(@"from {0} import {1};", From, Import);
            object obj = GetPyModule(pyImportString, Import);
            return obj;
        }

        private static string ReadStream(MemoryStream stream)
        {
            int length = (int)stream.Length;
            var bytes = new Byte[length];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(bytes, 0, (int)stream.Length);
            return Encoding.GetEncoding("utf-8").GetString(bytes, 0, (int)stream.Length);
        }


    }


}
