using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.PythonConverter
{
    //moduleDict = {"Name":module.__name__ ,"Classes":mod_classes, "Functions":mod_functions, "Valuables":mod_valuables};
    partial class PyModuleDescription
    {
        //Module's full name: like "honeybee.radiance.command.raBmp"
        public string Name;
        
        //Classes in this python module
        public List<PyClassDescription> Classes;

        //Functions in this python module
        public IList<dynamic> Functions;

        //Valuables in this python module
        public IList<dynamic> Valuables;



        //namespace, all caped names
        public string Namespace;
        //caped module name for saving the file
        public string ModuleName;

        //T4 generated code
        public string CSCode;

        public PyModuleDescription()
        {

        }

        public PyModuleDescription(dynamic PyModuleObject)
        {
            this.Name = PyModuleObject["Name"];
            var classes = PyModuleObject["Classes"] as IList<dynamic>;
            this.Functions = PyModuleObject["Functions"] as IList<dynamic>;
            this.Valuables = PyModuleObject["Valuables"] as IList<dynamic>;

            var capName = CheckName(this.Name);
            this.Namespace = GetNamespace(capName);
            this.ModuleName = GetModuleName(capName);

            this.Classes = (from classObj in classes select new PyClassDescription(classObj)).ToList();


            this.CSCode = this.TransformText();
        }

        private static string GetNamespace (string ModuleFullName)
        {
            var names = ModuleFullName.Split('.');
            return string.Join(".", names.Take(names.Length - 1));
        }

        private static string GetModuleName(string ModuleFullName)
        {
            return ModuleFullName.Split('.').Last();
        }

        private static string CheckName(string Name)
        {
            if (Name.Contains('.')) //check for namespaces
            {
                var names = Name.Split('.').Select(name => UpperInitial(name));
                return string.Join(".",names);
            }
            else
            {
                return UpperInitial(Name);
            }
        }

        // Static methods
        private static string UpperInitial(string name)
        {
            return name[0].ToString().ToUpperInvariant() + name.Substring(1);
        }

        private static string ReturnType(bool IfReturn)
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
        private static string CheckOverride(bool IfOverride)
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

    //classDict = {"Bases": baseNames,"Properties":properties,"Methods":methods, "Name": classObj.__name__};
    public class PyClassDescription
    {
        public string Name;
        public IList<dynamic> Properties;
        public IList<dynamic> Methods;
        //public List<string> Bases;

        public string NameCS;
        public string BaseClassNamesString;

        public PyClassDescription()
        {

        }
        public PyClassDescription(dynamic PyClassObj)
        {

            this.Name = PyClassObj["Name"];
            var bases = PyClassObj["Bases"] as IList<dynamic>;
            this.Properties = PyClassObj["Properties"] as IList<dynamic>;
            this.Methods = PyClassObj["Methods"] as IList<dynamic>;


            var baseNames = bases.Select(item => item["Name"]);
            //this.Bases = baseNames.ConvertAll<string>();
            this.BaseClassNamesString = string.Join(",", baseNames);

        }
    }




}
