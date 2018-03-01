using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Ironbug.Grasshopper.Properties;
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
            pManager.AddGenericParameter("supply", "supply", "heating or cooling supply source", GH_ParamAccess.list);
            pManager.AddGenericParameter("demand", "demand", "zoneMixer or other HVAC components", GH_ParamAccess.list);
            pManager[0].Optional = true;
            pManager[1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ZonesWithAirLoopHVAC", "ZoneHVAC", "toSaveOSM", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var supplyComs = new List<HVAC.IB_HVACObject>();
            DA.GetDataList(0, supplyComs);

            var demandComs = new List<HVAC.IB_ThermalZone>();
            DA.GetDataList(1, demandComs);

            var airLoop = new HVAC.IB_AirLoopHVAC();

            //TODO: need to check nulls
            foreach (var item in supplyComs)
            {
                airLoop.AddToSupplySide(item);
            }

            foreach (var item in demandComs)
            {
                airLoop.AddToDemandBranch(item);
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
                return Resources.AirLoop;
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