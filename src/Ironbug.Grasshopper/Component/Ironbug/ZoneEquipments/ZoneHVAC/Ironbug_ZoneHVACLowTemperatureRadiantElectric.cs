using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ZoneHVACLowTemperatureRadiantElectric : Ironbug_DuplicableHVACWithParamComponent
    {
        public Ironbug_ZoneHVACLowTemperatureRadiantElectric()
          : base("IB_ZoneHVACLowTemperatureRadiantElectric", "LowTRadiant_Elec",
              "Description",
              "Ironbug", "04:ZoneEquipments",
              typeof(HVAC.IB_ZoneHVACLowTemperatureRadiantElectric_FieldSet))
        {
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.RadiantElec;

        public override Guid ComponentGuid => new Guid("{C58FE0C0-D8E5-4E36-9092-DC9BA610FD57}");

        public override GH_Exposure Exposure => GH_Exposure.quarternary | GH_Exposure.obscure;
        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }
        
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ZoneHVACLowTemperatureRadiantElectric", "Radiant", "Connect to zone's equipment", GH_ParamAccess.item);
        }
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_ZoneHVACLowTemperatureRadiantElectric();

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }
        

    }
    
}