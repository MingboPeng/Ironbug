using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_OAController : Ironbug_HVACWithParamComponent
    {
        public Ironbug_OAController()
          : base("Ironbug_OAController", "OAController",
              "Description",
              "Ironbug", "06:Sizing&Controller",
              typeof(HVAC.IB_ControllerOutdoorAir_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("ControllerMechanicalVentilation", "MechVentCtrl_", "ControllerMechanicalVentilation", GH_ParamAccess.item);
            pManager[0].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("OutdoorAirSystemController", "OACtrl", "connect to OutdoorAirSystem", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_ControllerOutdoorAir();
            var mechVentCtrl = (HVAC.IB_ControllerMechanicalVentilation)null;
            
            if (DA.GetData(0, ref mechVentCtrl))
            {
                obj.SetMechanicalVentilation(mechVentCtrl);
            }

            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);

        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.OACtrl;

        
        public override Guid ComponentGuid => new Guid("8c5f0941-421e-428b-9b26-0d17280448fe");
    }
}