using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ironbug.Utilities;

namespace Ironbug.Core.Honeybee.Radiance.Command
{
    public class RaTiff : ICommandBase
    {
        private dynamic raTiff = null;
        public RaTiff(string HdrFile, string TiffFile)
        {
            
            PythonEngine engine = new PythonEngine();
            this.raTiff = engine.GetPyModule("RaTiff");

            if (this.raTiff != null)
            {
                this.raTiff = raTiff(HdrFile, TiffFile);
            }
            
        }


        public string Execute()
        {
            return this.raTiff.execute();
        }
    }
}
