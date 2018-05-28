﻿using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Ironbug.HVAC.BaseClass;
using Rhino.Geometry;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_SizingZone : Ironbug_HVACComponentBase
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_SizingZone class.
        /// </summary>
        public Ironbug_SizingZone()
          : base("Ironbug_SizingZone", "SzZone",
              "Description",
              "Ironbug", "06:Sizing&Controller",
              typeof(HVAC.IB_SizingZone_DataFieldSet))
        {
        }
        public override GH_Exposure Exposure => GH_Exposure.primary;

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
            pManager.AddGenericParameter("SizingZone", "SzZone", "SizingZone", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_SizingZone();

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
                return Properties.Resources.SizingZone;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("555eb7a9-bef9-48c8-abe7-32490f2d9aab"); }
        }
    }
}