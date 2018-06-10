﻿using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CoilCoolingCooledBeam : Ironbug_HVACComponentBase
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_CoilHeatingWater class.
        /// </summary>
        public Ironbug_CoilCoolingCooledBeam()
          : base("Ironbug_CoilCoolingCooledBeam", "Coil_CB",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_CoilCoolingCooledBeam_DataFieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

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
            pManager.AddGenericParameter("CoilCoolingCooledBeam", "Coil_CB", "connect to chilled beam", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_CoilCoolingCooledBeam();
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
                return Properties.Resources.Coil_CB;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("BDFFF56A-C2A0-4392-850B-8BE299CCD6F2"); }
        }
        
    }
    
}