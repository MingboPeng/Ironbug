using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.PythonConverter
{
    public class PyClassInfo
    {
        public string BaseClassName { get; set; }
        public string ClassName { get; set; }
        public List<PyPropergyInfo> Properties { get; set; }
        public PyConstuctorInfo Constuctor { get; set; }
        public List<PyMethodInfo> Methods { get; set; }
        public PyClassInfo()
        {
            this.Properties = new List<PyPropergyInfo>();
            this.Constuctor = new PyConstuctorInfo();
            this.Methods = new List<PyMethodInfo>();
        }
        public PyClassInfo(string ClassName)
        {
            this.ClassName = ClassName;
            

        }

    }

    public class PyPropergyInfo
    {
        public string Name { get; set; }
        public GetSet GetSet { get; set; }
    }

    public class PyConstuctorInfo
    {
        public string Names { get; set; }
        public List<PyValueInfo> Inputs { get; set; }
    }

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
    }

    public class PyValueInfo
    {
        public string Name { get; set; }
        public ValueTypes ValueType { get; set; }
    }

    public enum ValueTypes
    {
        String,
        Int,
        Float,
        Bool,
        Object,
        Void,
    }

    public enum GetSet
    {
        getter,
        setter,
    }
}
