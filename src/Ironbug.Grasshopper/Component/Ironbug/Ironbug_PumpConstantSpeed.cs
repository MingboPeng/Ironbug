﻿using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Ironbug.Grasshopper.Component.Ironbug
{
    public class Ironbug_PumpConstantSpeed : Ironbug_HVACComponent
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_PumpConstantSpeed class.
        /// </summary>
        public Ironbug_PumpConstantSpeed()
          : base("Ironbug_PumpConstantSpeed", "PumpConstant",
              "Description",
              "Ironbug", "01:LoopComponents",
              typeof(HVAC.IB_PumpConstantSpeed_DataFields))
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Parameters for detail settings", "params_", "Detail settings for this pump. Use Ironbug_ObjParams to set this.", GH_ParamAccess.item);
            pManager[0].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PumpConstantSpeed", "Pump", "connect to airloop's supply side", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_PumpConstantSpeed();

            var settingParams = new Dictionary<HVAC.IB_DataField, object>();
            DA.GetData(0, ref settingParams);

            obj.SetAttributes(settingParams);

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
            get { return new Guid("6c66ba6d-2e05-418b-9b13-8324a36c6388"); }
        }
    }
}