using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_HeaderedPumpsVariableSpeed : Ironbug_HVACComponent
    {
        public Ironbug_HeaderedPumpsVariableSpeed()
          : base("Ironbug_HeaderedPumpsVariableSpeed", "PumpVar_Headered",
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
            var obj = new HVAC.IB_HeaderedPumpsVariableSpeed();
            
            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.PumpV_Headered;//return null;

        public override Guid ComponentGuid => new Guid("20564A51-247D-4C25-98B1-D9F4E230A078");
    }
}