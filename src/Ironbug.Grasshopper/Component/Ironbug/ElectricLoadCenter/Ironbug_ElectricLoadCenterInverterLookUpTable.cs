using System;
using Grasshopper.Kernel;
using Ironbug.Grasshopper.Properties;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ElectricLoadCenterInverterLookUpTable : Ironbug_HVACWithParamComponent
    {
        public Ironbug_ElectricLoadCenterInverterLookUpTable()
          : base("IB_ElectricLoadCenterInverterLookUpTable", "InverterLookUpTable",
              "Description",
              "Ironbug", "08:ElectricLoadCenter",
              typeof(HVAC.IB_ElectricLoadCenterInverterLookUpTable_FieldSet))
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
            var obj = new HVAC.IB_ElectricLoadCenterInverterLookUpTable();

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }

        protected override System.Drawing.Bitmap Icon => Resources.InverterTable;

        public override Guid ComponentGuid => new Guid("A1B2C3D4-E5F6-7890-1234-567890ABCDEF");
    }
}