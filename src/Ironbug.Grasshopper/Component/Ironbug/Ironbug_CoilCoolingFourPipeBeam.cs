using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CoilCoolingFourPipeBeam : Ironbug_HVACComponent
    {
        
        /// Initializes a new instance of the Ironbug_CoilHeatingWater class.
        
        public Ironbug_CoilCoolingFourPipeBeam()
          : base("Ironbug_CoilCoolingFourPipeBeam", "Coil_Cln4PipBeam",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_CoilCoolingFourPipeBeam_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary | GH_Exposure.obscure;

        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CoilCoolingFourPipeBeam", "CoilC", "Connect to chilled beam", GH_ParamAccess.item);
            pManager.AddGenericParameter("WaterSide_Coil", "ToWaterLoop", "Connect to chilled water loop's demand side via plantBranches", GH_ParamAccess.item);
        }

        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_CoilCoolingFourPipeBeam();
            
            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
            DA.SetData(1, obj);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.Coil_Cooling4PB;


        public override Guid ComponentGuid => new Guid("{A71EB4F1-EB32-4CC1-A729-C3E4A6DB3616}");

    }
    
}