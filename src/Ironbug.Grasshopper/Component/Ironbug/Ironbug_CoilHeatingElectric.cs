using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Ironbug.HVAC.BaseClass;
using Rhino.Geometry;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CoilHeatingElectric : Ironbug_HVACComponent
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_CoilHeatingElectric class.
        /// </summary>
        public Ironbug_CoilHeatingElectric()
          : base("Ironbug_CoilHeatingElectric", "CoilHE",
              "Description",
              "Ironbug", "01:LoopComponents",
              typeof(HVAC.IB_CoilHeatingWater_DataFieldSet))
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Parameters", "params_", "Detail settings for this Coil. Use Ironbug_ObjParams to set this.", GH_ParamAccess.item);
            pManager[0].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CoilHeatingElectric", "CoilHE", "TODO:...", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_CoilHeatingElectric();

            var settingParams = new Dictionary<IB_DataField, object>();
            if (DA.GetData("Parameters", ref settingParams))
            {
                obj.SetAttributes(settingParams);
            }


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
                return Properties.Resources.CoilHE;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("dc8ed4d8-4435-4134-b1e8-06362bb6d411"); }
        }
    }
}