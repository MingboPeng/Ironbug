using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CoilHeatingFourPipeBeam : Ironbug_HVACComponent
    {
        
        /// Initializes a new instance of the Ironbug_CoilHeatingWater class.
        
        public Ironbug_CoilHeatingFourPipeBeam()
          : base("Ironbug_CoilHeatingFourPipeBeam", "Coil_Htn4PipBeam",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_CoilHeatingFourPipeBeam_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary | GH_Exposure.obscure;

        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CoilHeatingFourPipeBeam", "CoilH", "Connect to chilled beam", GH_ParamAccess.item);
            pManager.AddGenericParameter("WaterSide_Coil", "ToWaterLoop", "Connect to hot water loop's demand side via plantBranches", GH_ParamAccess.item);
        }

        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_CoilHeatingFourPipeBeam();
            
            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
            DA.SetData(1, obj);
        }

        protected override System.Drawing.Bitmap Icon => null;


        public override Guid ComponentGuid => new Guid("{838A3E2A-3FFA-4859-A024-15F9BA8FF4FC}");

    }
    
}