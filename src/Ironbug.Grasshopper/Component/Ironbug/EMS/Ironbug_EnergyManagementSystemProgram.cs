using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_EnergyManagementSystemProgram : Ironbug_Component
    {
        public Ironbug_EnergyManagementSystemProgram()
          : base("Ironbug_EMSProgram", "EMSProgram",
              "Description",
              "Ironbug", "06:Sizing&Controller")
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary | GH_Exposure.obscure;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("_name_", "_name_", "Name", GH_ParamAccess.item);
            pManager[0].Optional = true;
            pManager.AddTextParameter("_programBody", "_programBody", "_ProgramBody", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("program", "program", "Program", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_EnergyManagementSystemProgram();
            string name = null;
            if(DA.GetData(0, ref name))
                obj.SetName(name);

            string body = null;
            DA.GetData(1, ref body);
            obj.SetProgramBody(body);

            DA.SetData(0, obj);

        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.EMS_Program;

        
        public override Guid ComponentGuid => new Guid("CC0036A3-854E-44F9-8EA1-BFF57B5DBD46");
    }
}