using System;
using System.Collections.Generic;
using System.IO;
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
        public void ExportCSFile(string SaveTo)
        {
            //var usingStrings = new List<string>();
            //string[] lines = { this.ClassName, this.BaseClassName, "Third line" };
            File.WriteAllText(SaveTo, this.ToCsString());
        }

        public string ToCsString()
        {
            //var writeStrings = new List<string>();
            //writeStrings.Add(string.Format("public class {0}:CommandBase\n{{0}}", this.ClassName));
            //string constructorString = this.Constuctor.ToString();

            string classString = string.Format("public class {0}:{1}\n{{ \n{2} \n}}", ClassName, BaseClassName, this.Constuctor);

            return classString;
        }

        public override string ToString()
        {
            return ClassName + ":" + BaseClassName;
        }

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
