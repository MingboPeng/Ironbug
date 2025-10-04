using System;
using Grasshopper.Kernel;
using Ironbug.Grasshopper.Properties;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ElectricLoadCenterStorageSimple : Ironbug_HVACWithParamComponent
    {
        public Ironbug_ElectricLoadCenterStorageSimple()
          : base("IB_ElectricLoadCenterStorageSimple", "StorageSimple",
              "Description",
              "Ironbug", "08:ElectricLoadCenter",
              typeof(HVAC.IB_ElectricLoadCenterStorageSimple_FieldSet))
        {

        }
        public override GH_Exposure Exposure => GH_Exposure.secondary;


        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Storage", "Storage", "Electrical Storage", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_ElectricLoadCenterStorageSimple();

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }

        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("E5F6A7B8-C9D0-1234-5678-90ABCDEF1234");
    }
}