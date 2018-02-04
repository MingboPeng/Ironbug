using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_AirLoopHVAC : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_AirLoopHVAC class.
        /// </summary>
        public Ironbug_AirLoopHVAC()
          : base("Ironbug_AirLoopHVAC", "AirLoop",
              "Description",
              "Ironbug", "02:Loops")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("supply", "spl", "heating or cooling supply source", GH_ParamAccess.list);
            //pManager[0].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("AirLoopHVAC", "airLoop", "connect to airloop's demand side", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<HVAC.IIB_HVACComponent> supplyComs = new List<HVAC.IIB_HVACComponent>();
            DA.GetDataList(0, supplyComs);

            var airLoop = new HVAC.IB_AirLoopHVAC();

            //TODO: need to check nulls
            foreach (var item in supplyComs)
            {
                airLoop.AddToSupplyEnd(item);
            }
            

            //var model = new OpenStudio.Model();
            DA.SetData(0, airLoop);

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
            get { return new Guid("a416631f-bdda-4e11-8a2c-658c38681201"); }
        }
    }
}