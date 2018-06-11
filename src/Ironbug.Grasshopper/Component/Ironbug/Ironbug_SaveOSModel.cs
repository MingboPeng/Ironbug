using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_SaveOSModel : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_SaveOSModel class.
        /// </summary>
        public Ironbug_SaveOSModel()
          : base("Ironbug_SaveOSModel", "Nickname",
              "Description",
              "Ironbug", "HVAC")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("FilePath", "path", "file path", GH_ParamAccess.item);
            pManager.AddGenericParameter("AirLoops", "AirLoops", "Zone with HVAC system set", GH_ParamAccess.list);
            pManager.AddGenericParameter("PlantLoops", "PlantLoops", "PlantLoops", GH_ParamAccess.list);
            pManager.AddGenericParameter("VRFSystems", "VRFSystems", "VRFSystems", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Write", "_write", "Write the OpenStudio file.", GH_ParamAccess.item, false);

            pManager[0].Optional = true;
            pManager[1].Optional = true;
            pManager[1].DataMapping = GH_DataMapping.Flatten;
            pManager[2].Optional = true;
            pManager[2].DataMapping = GH_DataMapping.Flatten;
            pManager[3].Optional = true;
            pManager[3].DataMapping = GH_DataMapping.Flatten;

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("FilePath", "path", "file path", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string filepath = string.Empty;
            //filepath = @"C:\Users\mingo\Documents\GitHub\Ironbug\doc\osmFile\savedFromGH.osm";
            var airLoops = new List<HVAC.IB_AirLoopHVAC>();
            var plantLoops = new List<HVAC.IB_PlantLoop>();
            var vrfs = new List<HVAC.IB_AirConditionerVariableRefrigerantFlow>();
            bool write = false;

            //var model = new OpenStudio.Model();

            DA.GetData(0, ref filepath);
            DA.GetDataList(1, airLoops);
            DA.GetDataList(2,  plantLoops);
            DA.GetDataList(3, vrfs);

            DA.GetData(4, ref write);

            if (!write) return;
            
            if (string.IsNullOrEmpty(filepath)) return;

            var hvac = new HVAC.IB_HVACSystem(airLoops, plantLoops, vrfs);
            var saved = hvac.SaveHVAC(filepath);

            if (saved)
            {
                DA.SetData(0, filepath);
            }
            
            
            
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
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("3246f516-d4cf-45e0-b0a7-abb47bb014c1"); }
        }

        
    }
}