using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ZoneHVACEnergyRecoveryVentilatorController : Ironbug_HVACWithParamComponent
    {
        public Ironbug_ZoneHVACEnergyRecoveryVentilatorController()
          : base("IB_ZoneHVACEnergyRecoveryVentilatorController", "ZoneERVController",
              "Description",
              "Ironbug", "06:Sizing & Controller",
              typeof(HVAC.IB_ZoneHVACEnergyRecoveryVentilatorController_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary | GH_Exposure.obscure;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ZoneERVController", "ctrl", "connect to ZoneHVACEnergyRecoveryVentilator", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_ZoneHVACEnergyRecoveryVentilatorController();
            
            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }

        protected override System.Drawing.Bitmap Icon => null;
        public override Guid ComponentGuid => new Guid("966CAFE1-BB1B-4F6D-A4DA-7CF39CD4F41A");
    }
}