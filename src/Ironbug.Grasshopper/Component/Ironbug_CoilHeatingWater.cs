using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CoilHeatingWater : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_CoilHeatingWater class.
        /// </summary>
        public Ironbug_CoilHeatingWater()
          : base("Ironbug_CoilHeatingWater", "Nickname",
              "Description",
              "Ironbug", "01:LoopComponents")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("supply", "spl", "hot water supply source", GH_ParamAccess.item);
            pManager[0].Optional = true;
            pManager.AddNumberParameter("Inlet Water Temperature", "inWaterT[°C]", "Value in °C. [Default: 82.2 °C]", GH_ParamAccess.item, 82.2);
            pManager[1].Optional = true;
            pManager.AddNumberParameter("Outlet Water Temperature", "outWaterT[°C]", "Value in °C. [Default: 71.1 °C]", GH_ParamAccess.item, 71.1);
            pManager[2].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CoilHeatingWater", "CoilHW", "connect to airloop's supply side", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var coil = new HVAC.IB_CoilHeatingWater();

            double inWT = 0;
            double outWT = 0;

            DA.GetData(1, ref inWT);
            DA.GetData(2, ref outWT);
            
            coil.SetAttribute(HVAC.IB_CoilHeatingWater.AttributeNames.RatedInletWaterTemperature, inWT);
            coil.SetAttribute(HVAC.IB_CoilHeatingWater.AttributeNames.RatedOutletWaterTemperature, outWT);
            //coil.osCoilHeatingWater.setRatedInletWaterTemperature(inWT);
            //coil.osCoilHeatingWater.setRatedOutletWaterTemperature(outWT);



            DA.SetData(0, coil);
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
            get { return new Guid("4f849460-bb38-441c-9387-95c5be5830e7"); }
        }
    }
}