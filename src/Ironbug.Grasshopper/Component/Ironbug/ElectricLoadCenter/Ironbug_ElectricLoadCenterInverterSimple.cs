using System;
using Grasshopper.Kernel;
using Ironbug.Grasshopper.Properties;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ElectricLoadCenterInverterSimple : Ironbug_HVACWithParamComponent
    {
        public Ironbug_ElectricLoadCenterInverterSimple()
          : base("IB_ElectricLoadCenterInverterSimple", "InverterSimple",
              "Description",
              "Ironbug", "08:ElectricLoadCenter",
              typeof(HVAC.IB_ElectricLoadCenterInverterSimple_FieldSet))
        {

        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("inverter", "inverter", "inverter", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_ElectricLoadCenterInverterSimple();

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }

        protected override System.Drawing.Bitmap Icon => Resources.Inverter;

        public override Guid ComponentGuid => new Guid("B2C3D4E5-F6A7-8901-2345-67890ABCDEF1");
    }
}