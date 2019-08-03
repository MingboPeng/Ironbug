using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_FluidCoolerSingleSpeed : Ironbug_DuplicableHVACWithParamComponent
    {
        
        
        /// Initializes a new instance of the Ironbug_BoilerHotWater class.
        
        public Ironbug_FluidCoolerSingleSpeed()
          : base("Ironbug_FluidCoolerSingleSpeed", "FluidCooler",
              "Description",
              "Ironbug", "02:LoopComponents", 
              typeof(HVAC.IB_FluidCoolerSingleSpeed_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary | GH_Exposure.obscure;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("FluidCoolerSingleSpeed", "FluidCooler", "FluidCoolerSingleSpeed", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_FluidCoolerSingleSpeed();

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.FluidCooler2;

        public override Guid ComponentGuid => new Guid("{CF12CB91-E9D8-4E78-B4CD-01EB6A61DF6D}");
    }
}