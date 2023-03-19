using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ControllerWaterCoil : Ironbug_DuplicableHVACWithParamComponent
    {
        public Ironbug_ControllerWaterCoil()
          : base("IB_ControllerWaterCoil", "ControllerWaterCoil",
              "Description",
              "Ironbug", "06:Sizing & Controller",
              typeof(HVAC.IB_ControllerWaterCoil_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary | GH_Exposure.obscure;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ControllerWaterCoil", "Ctrl", "connect to advanced CoilCoolingWater or CoilHeatingWater", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_ControllerWaterCoil();

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);

        }
        protected override System.Drawing.Bitmap Icon => Properties.Resources.WaterCoilCtrl;

        public override Guid ComponentGuid => new Guid("ABF078CD-62C6-4820-8B5B-DC715041A32A");
    }
}