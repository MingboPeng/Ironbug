using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ChillerElectricEIR_Air : Ironbug_HVACComponent
    {
        public Ironbug_ChillerElectricEIR_Air()
          : base("Ironbug_ChillerElectricEIR_AirCooled", "Chiller_AirCooled",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_ChillerElectricEIR_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;
        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ChillerElectricEIR_AirCooled", "Chiller", "Connect to chilled water loop's supply side.", GH_ParamAccess.item);
        }

        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_ChillerElectricEIR();
            
            var objs = this.SetObjParamsTo(obj);
            DA.SetDataList(0, objs);
            
        }
        
        protected override System.Drawing.Bitmap Icon => Properties.Resources.Chiller_Air;
        
        public override Guid ComponentGuid => new Guid("BDE02476-2348-4258-9291-FAA0F48A16C0");
    }
}