using System;
using Grasshopper.Kernel;
using Ironbug.HVAC;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_FanZoneExhaust : Ironbug_HVACComponentBase
    {

        protected override System.Drawing.Bitmap Icon => Properties.Resources.exFan;

        public override Guid ComponentGuid => new Guid("FFCB9844-A425-4D06-A1B3-9C2ABBB5BF97");

        public override GH_Exposure Exposure => GH_Exposure.quarternary;
        
        public Ironbug_FanZoneExhaust()
          : base("Ironbug_FanZoneExhaust", "ExFan",
              "Description",
              "Ironbug", "04:ZoneEquipments",
              typeof(IB_FanZoneExhaust_DataFieldSet))
        {
        }

        
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
        }
        
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("FanZoneExhaust", "ExFan", "Connect to zone's equipment", GH_ParamAccess.item);
        }
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_FanZoneExhaust();
            
            
            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }
        
    }
}