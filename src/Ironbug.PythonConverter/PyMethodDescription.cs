using System;
using System.Collections.Generic;
using System.Linq;

namespace Ironbug.PythonConverter
{
    //funcDict = {"Type": type,"IfOverride":isOverride,"IfReturn":ifReturn, "Name": objName, "Arguments": arguments}
    public class PyMethodDescription
    {
        public string Name;
        public IEnumerable<PyValuableDescription> Arguments;
        public bool IfOverride;
        public bool IfReturn;
        public string Type; //Method, Function, Construtor

        
        public string NameCS;
        public string ReturnType;
        public string OverrideMark;

        public PyMethodDescription()
        {

        }
        public PyMethodDescription(dynamic PyMethodObj)
        {

            this.Name = PyMethodObj["Name"];
            var args = PyMethodObj["Arguments"] as IList<dynamic>;
            this.IfOverride = PyMethodObj["IfOverride"];
            this.IfReturn = PyMethodObj["IfReturn"];
            this.Type = PyMethodObj["Type"];


            this.NameCS = this.Name.CheckNamingForCS();
            this.ReturnType = GetReturnType(this.IfReturn);
            this.OverrideMark = GetOverrideKey(this.IfOverride);


            this.Arguments = args.Select(item => new PyValuableDescription(item));
        }

        private static string GetReturnType(bool IfReturn)
        {
            if (IfReturn)
            {
                return "object"; //use object for types for now. TODO: fix it later
            }
            else
            {
                return "void";
            }

        }

        private static string GetOverrideKey(bool IfOverride)
        {
            if (IfOverride)
            {
                return "override";
            }
            else
            {
                return string.Empty;
            }

        }
    }

    



}
