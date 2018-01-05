using System.Collections.Generic;
using System.Linq;


namespace Ironbug.PythonConverter
{
    //classDict = {"Bases": baseNames,"Properties":properties,"Methods":methods, "Name": classObj.__name__};
    public class PyClassDescription
    {
        public string Name;
        public List<PyProperty> Properties;
        public List<PyMethodDescription> Methods;
        //public List<string> Bases;

        public string NameCS;
        public string BaseClassNamesString;

        public PyClassDescription()
        {
            //this.Properties = new List<string>();
            //this.Methods = new List<PyMethodDescription>();
        }
        public PyClassDescription(dynamic PyClassObj)
        {

            this.Name = PyClassObj["Name"];
            var bases = PyClassObj["Bases"] as IList<dynamic>;
            var properties = PyClassObj["Properties"] as IList<dynamic>;
            var methods = PyClassObj["Methods"] as IList<dynamic>;


            this.NameCS = this.Name.CheckNamingForCS();
            this.Properties = properties.Select(item => new PyProperty(item)).ToList();
            this.Methods = methods.Select(item => new PyMethodDescription(item)).ToList();
            var baseNames = bases.Select(item => item["Name"]);
            //this.Bases = baseNames.ConvertAll<string>();
            this.BaseClassNamesString = string.Join(",", baseNames);

        }
    }

    public class PyProperty
    {
        public string Name;
        public string NameCS;
        public PyProperty(dynamic pyObj)
        {
            this.Name = pyObj;
            this.NameCS = this.Name.CheckNamingForCS();
        }
    }

    



}
