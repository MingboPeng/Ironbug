using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CoilCoolingCooledBeam : Ironbug_HVACComponent
    {
        
        /// Initializes a new instance of the Ironbug_CoilHeatingWater class.
        
        public Ironbug_CoilCoolingCooledBeam()
          : base("Ironbug_CoilCoolingCooledBeam", "Coil_CooledBeam",
              "Description",
              "Ironbug", "04:ZoneEquipments",
              typeof(HVAC.IB_CoilCoolingCooledBeam_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.quinary;

        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CoilCoolingCooledBeam", "Coil_CB", "Connect to chilled beam", GH_ParamAccess.item);
            pManager.AddGenericParameter("WaterSide_CoilCoolingWater", "ToWaterLoop", "Connect to chilled water loop's demand side via plantBranches", GH_ParamAccess.item);
        }

        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_CoilCoolingCooledBeam();
            

            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
            DA.SetData(1, obj);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.Coil_CB;


        public override Guid ComponentGuid => new Guid("BDFFF56A-C2A0-4392-850B-8BE299CCD6F2");

    }
    
}