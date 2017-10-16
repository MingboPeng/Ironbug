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
    }
}
