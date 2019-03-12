using System;
using Grasshopper.Kernel;
using Ironbug.Grasshopper.Properties;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ExistPlantLoop : Ironbug_Component
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_PlantLoop class.
        /// </summary>
        public Ironbug_ExistPlantLoop()
          : base("Ironbug_ExistPlantLoop", "ExistPlantLoop",
              EPDoc.PlantLoop.Note,
              "Ironbug", "01:Loops")
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("OsPlantLoop", "OsPlantLoop", "The existing loop from Ironbug_ImportOSM component", GH_ParamAccess.item);
            pManager.AddGenericParameter("DemandPlantBranch", "demandBranch", "HVAC components in plantBranch to be added to this existing loop", GH_ParamAccess.item);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PlantLoop", "PlantLoop", "PlantLoop", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            HVAC.BaseClass.IB_ExistingObj name = null;
            DA.GetData(0, ref name);

            var demandComs = new HVAC.IB_PlantLoopBranches();
            DA.GetData(1, ref demandComs);
            
            var plant = new HVAC.IB_ExistPlantLoop(name);
            plant.AddBranches(demandComs);
            
            DA.SetData(0, plant);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Resources.PlantLoop_Exist;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("54712A3C-4D26-4739-95CD-2E591F22D009");

    }
}