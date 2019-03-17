using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_DistrictCooling : Ironbug_HVACComponent
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_DistrictCooling class.
        /// </summary>
        public Ironbug_DistrictCooling()
          : base("Ironbug_DistrictCooling", "DistrictCooling",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_DistrictCooling_DataFieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;


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
            pManager.AddGenericParameter("DistrictCooling", "DistCooling", "DistrictCooling for plant loop's supply.", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_DistrictCooling();
            
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
                return Properties.Resources.DistricCooling;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("96db6f68-01ad-44a9-bb0b-bf30459c8fbe"); }
        }
    }
}