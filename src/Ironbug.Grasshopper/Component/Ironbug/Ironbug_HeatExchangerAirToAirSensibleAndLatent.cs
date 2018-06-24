using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_HeatExchangerAirToAirSensibleAndLatent : Ironbug_HVACComponentBase
    {
        public Ironbug_HeatExchangerAirToAirSensibleAndLatent()
          : base("Ironbug_HeatExchangerAirToAirSensibleAndLatent", "HeatExg_Air",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_HeatExchangerAirToAirSensibleAndLatent_DataFieldSet))
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
            pManager.AddGenericParameter("HeatExchangerAirToAirSensibleAndLatent", "HeatExg_Air", "to OutdoorAirSystem", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_HeatExchangerAirToAirSensibleAndLatent();

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
                return Properties.Resources.HeatEx_Air;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("7E61D04D-038A-4ED3-BA95-CC2AA6DC5AC2"); }
        }
    }
}