using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ControllerMechanicalVentilation : Ironbug_DuplicatableHVACWithParamComponent
    {
        public Ironbug_ControllerMechanicalVentilation()
          : base("Ironbug_ControllerMechanicalVentilation", "MechVentController",
              "Description",
              "Ironbug", "06:Sizing&Controller",
              typeof(HVAC.IB_ControllerMechanicalVentilation_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            
            pManager.AddGenericParameter("ControllerMechanicalVentilation", "Ctrl", "connect to OAController", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_ControllerMechanicalVentilation();

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);

        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.MechVentCtrl;

        public override Guid ComponentGuid => new Guid("51B8F4A2-A0D8-49F6-8FD9-4F6D97711965");
    }
}