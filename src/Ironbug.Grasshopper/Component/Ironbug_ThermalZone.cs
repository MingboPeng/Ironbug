using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ThermalZone : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_ThermalZone class.
        /// </summary>
        public Ironbug_ThermalZone()
          : base("Ironbug_ThermalZone", "Nickname",
              "Description",
              "Ironbug", "01:LoopComponents")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("OpenStudio ThermalZone", "OSZone", "connect to airloop's demand side", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var zone = new HVAC.IB_ThermalZone();
            zone.AirTerminal = new HVAC.IB_AirTerminal();

            DA.SetData(0, zone);
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
            get { return new Guid("8aa3ced0-54bb-4cc3-b53b-9b63dbe714a0"); }
        }
    }
}