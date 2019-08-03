using System;

using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CoilCoolingDXMultiSpeed : Ironbug_DuplicatableHVACComponent
    {
        
        /// Initializes a new instance of the Ironbug_ChillerElectricEIR class.
        
        public Ironbug_CoilCoolingDXMultiSpeed()
          : base("Ironbug_CoilCoolingDXMultiSpeed", "CoilClnDXM",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_CoilCoolingDXMultiSpeed_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary | GH_Exposure.obscure;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CoilCoolingDXMultiSpeed", "Coil", "CoilCoolingDXMultiSpeed", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_CoilCoolingDXMultiSpeed();
            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
            this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "This obj is not fully finished by OpenStudio, stay tuned!");
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.CoilCDXM;


        public override Guid ComponentGuid => new Guid("E76307B8-27F4-4C86-AB23-0864160B725E");
    }
}