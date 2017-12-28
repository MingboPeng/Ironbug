using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.PythonConverter
{
    public class PyMethodInfo
    {
        public string Name { get; set; }
        public List<PyValueInfo> Inputs { get; set; }
        public ValueTypes ReturnTypes { get; set; }
        public PyMethodInfo()
        {
            this.Inputs = new List<PyValueInfo>();
            this.ReturnTypes = ValueTypes.Void;
        }

        public override string ToString()
        {
            return Name;
        }

        public string ToCsString()
        {
            var inputString = String.Join(",", Inputs);
            var inputStringWithTypes = String.Join(",", Inputs.Select(_ => _.ToString(WithType: true)));

            var header = String.Format("\tpublic {0} ({1})", Name, inputStringWithTypes);
            var lines = new List<string>();
            lines.Add(header);
            lines.Add("{");
            lines.Add(String.Format("\treturn RawObj.{0}({1});", Name, inputString));
            lines.Add("}");

            var bodyString = String.Join("\n\t", lines);


            return bodyString;
        }
    }
}
