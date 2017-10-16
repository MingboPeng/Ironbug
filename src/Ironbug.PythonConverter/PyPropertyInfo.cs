using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.PythonConverter
{
    public class PyPropergyInfo
    {
        public string Name { get; set; }
        public GetSet GetSet { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}
