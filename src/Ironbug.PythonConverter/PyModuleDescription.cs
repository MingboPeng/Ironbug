using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.PythonConverter
{
    partial class PyModuleDescription
    {
        //Module's name
        public string Name;

        //Classes in this python module
        public IList<dynamic> Classes;

        //Functions in this python module
        public IList<dynamic> Functions;

        //Valuables in this python module
        public IList<dynamic> Valuables;

        

        public PyModuleDescription(string ModuleName)
        {
            this.Name = ModuleName;
        }

    }
}
