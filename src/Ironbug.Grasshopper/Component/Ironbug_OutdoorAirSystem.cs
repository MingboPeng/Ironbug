using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_OutdoorAirSystem : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_OutdoorAirSystem class.
        /// </summary>
        public Ironbug_OutdoorAirSystem()
          : base("Ironbug_OutdoorAirSystem", "Nickname",
              "Description",
              "Ironbug", "01:LoopComponents")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Controller", "Controller_", "Controller for OutdoorAirSystem", GH_ParamAccess.item);
            pManager[0].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("OutdoorAirSystem", "OASystem", "OutdoorAirSystem", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_OutdoorAirSystem();

            var controller = new HVAC.IB_ControllerOutdoorAir();
            DA.GetData(0, ref controller);

            obj.ControllerOutdoorAir = controller;

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
            get { return new Guid("648436b4-3ce6-4db2-a1a5-91d9a2999e9f"); }
        }
    }
}