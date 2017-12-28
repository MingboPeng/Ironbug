using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.PythonConverter
{
    public class PyValueInfo
    {
        public string Name { get; set; }
        public ValueTypes ValueType { get; set; }
        public override string ToString()
        {
            return Name;
        }
        public string ToString(bool WithType)
        {
            return String.Format("{0} {1}", ValueType, Name);
        }



    }
}
