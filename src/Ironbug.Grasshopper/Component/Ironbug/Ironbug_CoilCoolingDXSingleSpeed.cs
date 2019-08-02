using System;

using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CoilCoolingDXSingleSpeed : Ironbug_LoopObjectComponent
    {
        
        /// Initializes a new instance of the Ironbug_ChillerElectricEIR class.
        
        public Ironbug_CoilCoolingDXSingleSpeed()
          : base("Ironbug_CoilCoolingDXSingleSpeed", "CoilClnDX1",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_CoilCoolingDXSingleSpeed_FieldSet))
        {
        }
        public override GH_Exposure Exposure => GH_Exposure.secondary;
        
        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CoilCoolingDXSingleSpeed", "Coil", "CoilCoolingDXSingleSpeed", GH_ParamAccess.item);
        }

        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_CoilCoolingDXSingleSpeed();
            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }


        protected override System.Drawing.Bitmap Icon => Properties.Resources.CoilCDX1;

        public override Guid ComponentGuid => new Guid("32CB9D9F-0328-4CDE-84F8-D2F36D9A2F07");
    }
}