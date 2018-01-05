using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Ironbug.PythonConverter
{
    //moduleDict = {"Name":module.__name__ ,"Classes":mod_classes, "Functions":mod_functions, "Valuables":mod_valuables};
    partial class PyModuleDescription
    {
        //Module's full name: like "honeybee.radiance.command.raBmp"
        public string Name { get; set; }

        //Classes in this python module
        public List<PyClassDescription> Classes { get; set; }

        //Functions in this python module
        public List<PyMethodDescription> Functions { get; set; }

        //Valuables in this python module
        public List<PyValuableDescription> Valuables { get; set; }



        //namespace, all caped names
        public string Namespace { get; set; }
        //caped module name for saving the file
        public string ModuleName { get; set; }

        //T4 generated code
        public string CSCode { get; set; }

        public PyModuleDescription()
        {
            this.Classes = new List<PyClassDescription>();
            this.Functions = new List<PyMethodDescription>();
            this.Valuables = new List<PyValuableDescription>();
        }

        public PyModuleDescription(dynamic PyModuleObject)
        {
            this.Name = PyModuleObject["Name"];
            var classes = PyModuleObject["Classes"] as IList<dynamic>;
            var functions = PyModuleObject["Functions"] as IList<dynamic>;
            var valuables = PyModuleObject["Valuables"] as IList<dynamic>;

            var capName = this.Name.CheckNamingForCS();
            this.Namespace = GetNamespace(capName);
            this.ModuleName = GetModuleName(capName);
            

            //foreach (var item in classes)
            //{
            //    var cls = new PyClassDescription(item);
            //    this.Classes.Add(cls);
            //}

            this.Classes = classes.Select(item => new PyClassDescription(item)).ToList();
            this.Functions = functions.Select(item => new PyMethodDescription(item)).ToList();
            this.Valuables = valuables.Select(item => new PyValuableDescription(item)).ToList();

            this.CSCode = this.TransformText();
        }



        private static string GetNamespace(string ModuleFullName)
        {
            var names = ModuleFullName.Split('.');
            return string.Join(".", names.Take(names.Length - 1));
        }

        private static string GetModuleName(string ModuleFullName)
        {
            return ModuleFullName.Split('.').Last();
        }




    }
}