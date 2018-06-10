using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Ironbug.HVAC;
using Ironbug.HVAC.BaseClass;
using Rhino.Geometry;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_AirTerminalSingleDuctUncontrolled : Ironbug_HVACComponentBase
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_AirTerminalSingleDuctVAVReheat class.
        /// </summary>
        public Ironbug_AirTerminalSingleDuctUncontrolled()
          : base("Ironbug_AirTerminalSingleDuctUncontrolled", "Diffuser",
              "Description",
              "Ironbug", "03:AirTerminals",
              typeof(IB_AirTerminalSingleDuctUncontrolled_DataFieldSet))
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
            pManager.AddGenericParameter("AirTerminalSingleDuctUncontrolled", "Diffuser", "connect to Zone", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new IB_AirTerminalSingleDuctUncontrolled();
            obj.PuppetEventHandler += PuppetStateChanged;
            
            this.SetObjParamsTo(obj);
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
                return Properties.Resources.AirTerminalUncontrolled;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("623EC8EE-FE37-44B7-BBC7-2BA62C597BC4"); }
        }
    }
}