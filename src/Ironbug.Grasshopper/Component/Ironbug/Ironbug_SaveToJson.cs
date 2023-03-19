using System;
using System.IO;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_SaveToJson : Ironbug_Component
    {
        public Ironbug_SaveToJson()
          : base("IB_SaveToJson", "SaveToJson",
              "Save HVACSystem to Json file",
              "Ironbug", "HVAC")
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary | GH_Exposure.obscure;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("_JsonPath", "_JsonPath", "New Json file path.", GH_ParamAccess.item);
            pManager.AddGenericParameter("HVACSystem", "HVAC", "A HVAC system from Ironbug_HVACSystem", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Write", "_write", "Write the file.", GH_ParamAccess.item, false);
        }


        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("FilePath", "path", "file path", GH_ParamAccess.item);
        }

        
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            string filepath = string.Empty;
            HVAC.IB_HVACSystem sys = null;
            bool write = false;

            DA.GetData(0, ref filepath);
            DA.GetData(1, ref sys);
            DA.GetData(2, ref write);

            if (!write) return;

            if (string.IsNullOrEmpty(filepath)) return;

            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }


            var json = sys.ToJson();
            var sys2 = HVAC.IB_HVACSystem.FromJson(json);
            if (sys != sys2)
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Failed to convert to json");
                return;
            }

            File.WriteAllText( filepath, json);
            if (!File.Exists(filepath))
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Failed to convert to json");
            }

            DA.SetData(0, filepath);


        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.ToJson;

        public override Guid ComponentGuid => new Guid("FDE5126C-9BF1-4790-A30A-6D93FDA89DBE");

    }

   
}