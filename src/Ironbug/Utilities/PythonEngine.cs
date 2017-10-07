using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Ironbug.Utilities
{
    public class PythonEngine
    {
        private ScriptEngine _engine;
        private MemoryStream _ms;
        public PythonEngine()
        {
            //create Engine
            this._engine = Python.CreateEngine();
            
            var sourceLibs = this._engine.GetSearchPaths();
            sourceLibs.Add(@"C:\Python27\Lib");
            sourceLibs.Add(@"C:\Users\Mingbo\Documents\GitHub\HoneybeeCSharp\src\Ironbug\Python");
            this._engine.SetSearchPaths(sourceLibs);

            _ms = new MemoryStream();
            _engine.Runtime.IO.SetOutput(_ms, new StreamWriter(_ms));
        }

        public object GetPyModule(string HBClass)
        {
            string pyModuleName = HBClass;
            
            try
            {
                //import HoneybeePlus module
                var pyImportString = string.Format(@"from honeybee.radiance.command.raTiff import {0};", pyModuleName);
                
                ScriptSource source = this._engine.CreateScriptSourceFromString(pyImportString);
                ScriptScope scope = this._engine.CreateScope();
                source.Execute(scope);
                
                //var code = source.Compile();

                string outputStrings = ReadStream(_ms);
                dynamic obj = scope.GetVariable(pyModuleName);
                return obj;
            }
            catch (Exception ex)
            {
                var outputStreamWriter = new StreamWriter(_ms);
                outputStreamWriter.Write(ex.ToString());
                return null;
            }
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
