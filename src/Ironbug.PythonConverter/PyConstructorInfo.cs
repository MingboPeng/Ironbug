using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.PythonConverter
{
    public class PyConstuctorInfo
    {
        public string Name { get; set; }
        public List<PyValueInfo> Inputs { get; set; }
        public PyConstuctorInfo()
        {
            this.Inputs = new List<PyValueInfo>();
        }

        //TODO: finish Import string.
        public override string ToString()
        {
            var inputString = String.Join(",", Inputs);
            var header = String.Format("\tpublic {0} ({1})", Name, inputString);
            var lines = new List<string>();
            lines.Add(header);
            lines.Add("{");
            lines.Add("\tPythonEngine engine = new PythonEngine();");
            lines.Add(String.Format("\tdynamic pyModule = engine.ImportFrom(From: \"honeybee.radiance.command.falsecolor\", Import: \"{0}\");", Name));
            lines.Add("\tif (pyModule != null)");
            lines.Add("\t{");
            lines.Add("\t\tthis.RawObj = pyModule(HdrFile, TiffFile);");
            lines.Add("\t}");
            lines.Add("}");

            var bodyString = String.Join("\n\t", lines);


            return bodyString;
        }
    }
}
