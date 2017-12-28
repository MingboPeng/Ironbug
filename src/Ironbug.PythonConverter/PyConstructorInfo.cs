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
        public string ToCsString()
        {
            var inputString = String.Join(",", Inputs);
            var inputStringWithTypes = String.Join(",", Inputs.Select(_ => _.ToString(WithType: true)));

            var header = String.Format("\tpublic {0} ({1})", Name, inputStringWithTypes);
            var lines = new List<string>();
            lines.Add(header);
            lines.Add("{");
            lines.Add("\tPythonEngine engine = new PythonEngine();");
            lines.Add(String.Format("\tdynamic pyModule = engine.ImportFrom(From: \"honeybee.radiance.command.falsecolor\", Import: \"{0}\");", Name));
            lines.Add("\tif (pyModule != null)");
            lines.Add("\t{");
            lines.Add(String.Format("\t\tthis.RawObj = pyModule({0});",inputString));
            lines.Add("\t}");
            lines.Add("}");

            var bodyString = String.Join("\n\t", lines);


            return bodyString;
        }
    }
}
