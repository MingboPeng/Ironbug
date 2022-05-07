using System;
using Grasshopper.Kernel;
using Ironbug.Grasshopper.Properties;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_PipeAdiabatic : Ironbug_DuplicableHVACWithParamComponent
    {
        public Ironbug_PipeAdiabatic()
          : base("IB_PipeAdiabatic", "PipeAdiabatic",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_PipeAdiabatic_FieldSet))
        {
            
        }

        public override GH_Exposure Exposure => GH_Exposure.septenary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PipeAdiabatic", "Pipe", "PipeAdiabatic", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_PipeAdiabatic();

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }

        protected override System.Drawing.Bitmap Icon => Resources.PipeAdiabatic;

        public override Guid ComponentGuid => new Guid("7CF7FC5F-622C-4F9E-A9C9-F21B01868DDF");


    }

   
}