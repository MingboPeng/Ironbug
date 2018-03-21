using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Ironbug.HVAC;
using Rhino.Geometry;

namespace Ironbug.Grasshopper.Component.Ironbug
{
    public class Ironbug_AirTerminalSingleDuctVAVReheat : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_AirTerminalSingleDuctVAVReheat class.
        /// </summary>
        public Ironbug_AirTerminalSingleDuctVAVReheat()
          : base("Ironbug_AirTerminalSingleDuctVAVReheat", "VAVReheat",
              "Description",
              "Ironbug", "01:LoopComponents")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Heating Source", "source", "HVAC components", GH_ParamAccess.item);
            pManager[0].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("AirTerminalSingleDuctVAVReheat", "VAVReheat", "connect to Zone", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new IB_AirTerminalSingleDuctVAVReheat();
            HVAC.IB_CoilHeatingWater heatingSource = null;

            DA.GetData(0, ref heatingSource);
            if (heatingSource != null)
            {
                var newHS = (IB_CoilHeatingWater)heatingSource.Duplicate();
                obj.SetReheatCoil(newHS);
            }


            DA.SetData(0, obj);

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
            get { return new Guid("aaf86609-f508-4fb2-9ed4-a8323e9549bd"); }
        }
    }
}