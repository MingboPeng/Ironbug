using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_HeaderedPumpsConstantSpeed : Ironbug_HVACComponent
    {
        public Ironbug_HeaderedPumpsConstantSpeed()
          : base("Ironbug_HeaderedPumpsConstantSpeed", "PumpConst_Headered",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_HeaderedPumpsVariableSpeed_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.quinary | GH_Exposure.obscure;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PumpConst_Headered", "Pump", "connect to plantloop's supply side", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_HeaderedPumpsConstantSpeed();
            
            var objs = this.SetObjParamsTo(obj);
            DA.SetDataList(0, objs);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.PumpC_Headered;

        public override Guid ComponentGuid => new Guid("E9857ABA-0E0E-4921-A525-8EC777E4D553");
    }
}