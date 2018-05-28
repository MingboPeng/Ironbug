﻿using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Ironbug.Grasshopper.Component.Ironbug
{
    public class Ironbug_CreatePuppets : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_CreatePuppets class.
        /// </summary>
        public Ironbug_CreatePuppets()
          : base("Ironbug_CreatePuppets", "Puppets",
              "Description",
              "Ironbug", "00:Ironbug")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Reference", "ref", "a reference obj for creating puppets", GH_ParamAccess.item);
            pManager.AddNumberParameter("Amount", "n", "number of puppets to be created", GH_ParamAccess.item, 1.0 );
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Puppets", "puppets", "Puppets", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            HVAC.BaseClass.IB_ModelObject obj = null;
            double amount = 1;
            DA.GetData(0, ref obj);
            DA.GetData(1, ref amount);

            if (obj !=null)
            {

                var puppets = new List<HVAC.BaseClass.IB_ModelObject>();
                for (int i = 0; i < amount; i++)
                {
                    var puppet = obj.Duplicate();
                    puppet.SetTrackingID();
                    puppets.Add(puppet);

                }

                DA.SetDataList(0, puppets);
                
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
            get { return new Guid("cb1cb9d6-e29e-4d26-9133-82b47c0e6d8d"); }
        }
    }
}