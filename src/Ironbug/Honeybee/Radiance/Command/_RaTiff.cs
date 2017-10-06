using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.Honeybee.Radiance.Command
{
    public class _RaTiff : ICommandBase
    {
        private dynamic RaTiff = null;
        public _RaTiff(string HdrFile, string TiffFile)
        {
            

            ScriptEngine engine = Python.CreateEngine();
            var sourceLibs = engine.GetSearchPaths();
            sourceLibs.Add(@"C:\Python27\Lib");
            sourceLibs.Add(@"C:\Users\Mingbo\Documents\GitHub\HoneybeeCSharp\src\Ironbug\Python");
            engine.SetSearchPaths(sourceLibs);

            //import HoneybeePlus module
            ScriptSource source = engine.CreateScriptSourceFromString(@"from honeybee.radiance.command.raTiff import RaTiff;");
            ScriptScope scope = engine.CreateScope();

            source.Execute(scope);
            this.RaTiff = scope.GetVariable("RaTiff");
            if (this.RaTiff != null)
            {
                //dynamic tiff = outObj(inHdr, outTiff).execute();
                this.RaTiff = RaTiff(HdrFile, TiffFile);
            }
            
        }


        public string Execute()
        {
            return this.RaTiff.execute();
        }
    }
}
