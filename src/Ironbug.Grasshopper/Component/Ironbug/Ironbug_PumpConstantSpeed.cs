using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_PumpConstantSpeed : Ironbug_HVACComponent
    {
        public Ironbug_PumpConstantSpeed()
          : base("Ironbug_PumpConstantSpeed", "PumpConstant",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_PumpConstantSpeed_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.quinary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PumpConstantSpeed", "Pump", "connect to plantloop's supply side", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_PumpConstantSpeed();

            
            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.PumpC;

        public override Guid ComponentGuid => new Guid("6c66ba6d-2e05-418b-9b13-8324a36c6388");
    }
}