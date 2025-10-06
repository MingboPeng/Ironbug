using System;
using Grasshopper.Kernel;
using Ironbug.Grasshopper.Properties;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ElectricLoadCenterStorageLiIonNMCBattery : Ironbug_HVACWithParamComponent
    {
        public Ironbug_ElectricLoadCenterStorageLiIonNMCBattery()
          : base("IB_ElectricLoadCenterStorageLiIonNMCBattery", "LiIonNMCBattery",
              "Description",
              "Ironbug", "08:ElectricLoadCenter",
              typeof(HVAC.IB_ElectricLoadCenterStorageLiIonNMCBattery_FieldSet))
        {

        }
        public override GH_Exposure Exposure => GH_Exposure.secondary;


        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("storage", "storage", "Electrical Storage", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_ElectricLoadCenterStorageLiIonNMCBattery();

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }

        protected override System.Drawing.Bitmap Icon => Resources.StorageNMCBattery;

        public override Guid ComponentGuid => new Guid("D4E5F6A7-B8C9-0123-4567-890ABCDEF123");
    }
}