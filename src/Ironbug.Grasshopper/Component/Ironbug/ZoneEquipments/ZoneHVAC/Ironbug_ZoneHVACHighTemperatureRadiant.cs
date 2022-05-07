using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ZoneHVACHighTemperatureRadiant : Ironbug_DuplicableHVACWithParamComponent
    {
        public Ironbug_ZoneHVACHighTemperatureRadiant()
          : base("IB_ZoneHVACHighTemperatureRadiant", "HighTRadiant",
              "Description",
              "Ironbug", "04:ZoneEquipments",
              typeof(HVAC.IB_ZoneHVACHighTemperatureRadiant_FieldSet))
        {
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.RadiantHigh;

        public override Guid ComponentGuid => new Guid("{5372CB92-A671-44DB-87F2-23A86AE8097D}");

        public override GH_Exposure Exposure => GH_Exposure.quarternary | GH_Exposure.obscure;
        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }
        
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ZoneHVACHighTemperatureRadiant", "Radiant", "Connect to zone's equipment", GH_ParamAccess.item);
        }
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_ZoneHVACHighTemperatureRadiant();

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }
        

    }
    
}