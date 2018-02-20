using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Ironbug.Grasshopper.Component.Ironbug
{
    public class Ironbug_BoilerHotWater : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_BoilerHotWater class.
        /// </summary>
        public Ironbug_BoilerHotWater()
          : base("Ironbug_BoilerHotWater", "Nickname",
              "Description",
              "Category", "Subcategory")
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
            pManager.AddGenericParameter("BoilerHotWater", "Boiler", "connect to plantloop's supply side", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_BoilerHotWater();

            //var settingParams = new Dictionary<HVAC.IB_DataField, object>();
            //DA.GetData(0, ref settingParams);

            //foreach (var item in settingParams)
            //{
            //    try
            //    {
            //        obj.SetAttribute(item.Key, item.Value);
            //    }
            //    catch (Exception)
            //    {

            //        throw;
            //    }
            //}

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
            get { return new Guid("5281d0b8-492e-4c52-a372-d9a63a94b4df"); }
        }
    }
}