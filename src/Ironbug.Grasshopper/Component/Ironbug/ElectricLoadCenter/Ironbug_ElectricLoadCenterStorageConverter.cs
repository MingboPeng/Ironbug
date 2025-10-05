using System;
using Grasshopper.Kernel;
using Ironbug.Grasshopper.Properties;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ElectricLoadCenterStorageConverter : Ironbug_HVACWithParamComponent
    {
        public Ironbug_ElectricLoadCenterStorageConverter()
          : base("IB_ElectricLoadCenterStorageConverter", "StorageConverter",
              "Description",
              "Ironbug", "08:ElectricLoadCenter",
              typeof(HVAC.IB_ElectricLoadCenterStorageConverter_FieldSet))
        {

        }
        public override GH_Exposure Exposure => GH_Exposure.secondary;


        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Converter", "Converter", "Storage Converter", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_ElectricLoadCenterStorageConverter();

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }

        protected override System.Drawing.Bitmap Icon => Resources.Converter;

        public override Guid ComponentGuid => new Guid("C3D4E5F6-A7B8-9012-3456-7890ABCDEF12");
    }
}