using System;
using Grasshopper.Kernel;
using Ironbug.HVAC;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ZoneHVACIdealLoadsAirSystem : Ironbug_HVACComponent
    {

        protected override System.Drawing.Bitmap Icon => Properties.Resources.exFan;

        public override Guid ComponentGuid => new Guid("9D9F8E07-254B-45BE-AFD0-B8770438EE2E");

        public override GH_Exposure Exposure => GH_Exposure.tertiary;
        
        public Ironbug_ZoneHVACIdealLoadsAirSystem()
          : base("Ironbug_ZoneHVACIdealLoadsAirSystem", "IdealAirLoad",
              "Description",
              "Ironbug", "04:ZoneEquipments",
              typeof(IB_ZoneHVACIdealLoadsAirSystem_FieldSet))
        {
        }

        
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
        }
        
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ZoneHVACIdealLoadsAirSystem", "IdealAirLoad", "Connect to zone's equipment", GH_ParamAccess.item);
        }
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_ZoneHVACIdealLoadsAirSystem();
            
            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }
        
    }
}