using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ironbug.Utilities;

namespace Ironbug.Honeybee.Radiance.Command
{
    public class _RaTiff : ICommandBase
    {
        private dynamic RaTiff = null;
        public _RaTiff(string HdrFile, string TiffFile)
        {
            
            PythonEngine engine = new PythonEngine();
            this.RaTiff = engine.GetPyModule("RaTiff");

            if (this.RaTiff != null)
            {
                this.RaTiff = RaTiff(HdrFile, TiffFile);
            }
            
        }


        public string Execute()
        {
            return this.RaTiff.execute();
        }
    }
}
