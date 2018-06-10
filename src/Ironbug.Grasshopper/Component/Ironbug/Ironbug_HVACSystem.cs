using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_HVACSystem : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_SaveOSModel class.
        /// </summary>
        public Ironbug_HVACSystem()
          : base("Ironbug_HVACSystem", "HVACSystem",
              "Description",
              "Ironbug", "HVAC")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("AirLoops", "AirLoops", "AirLoops", GH_ParamAccess.list);
            pManager.AddGenericParameter("PlantLoops", "PlantLoops", "PlantLoops", GH_ParamAccess.list);
            pManager.AddGenericParameter("VRFSystems", "VRFSystems", "VRFSystems", GH_ParamAccess.list);

            pManager[0].Optional = true;
            pManager[0].DataMapping = GH_DataMapping.Flatten;
            pManager[1].Optional = true;
            pManager[1].DataMapping = GH_DataMapping.Flatten;
            pManager[2].Optional = true;
            pManager[2].DataMapping = GH_DataMapping.Flatten;

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("HVACSystem", "HVACSystem", "A fully detailed HVAC system for ExportToOpenStudio component.", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var airLoops = new List<HVAC.IB_AirLoopHVAC>();
            var plantLoops = new List<HVAC.IB_PlantLoop>();
            var vrfs = new List<HVAC.IB_AirConditionerVariableRefrigerantFlow>();

            DA.GetDataList(0, airLoops);
            DA.GetDataList(1, plantLoops);
            DA.GetDataList(2, vrfs);
            
            var hvac = new HVAC.IB_HVACSystem(airLoops, plantLoops, vrfs);
            DA.SetData(0, hvac);


        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return Properties.Resources.HVAC;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("330C6DCC-EC73-49C7-96CB-B0EB522A1585"); }
        }

        
    }
}