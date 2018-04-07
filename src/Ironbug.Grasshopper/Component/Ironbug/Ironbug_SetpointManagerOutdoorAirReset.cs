using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Ironbug.HVAC.BaseClass;
using Rhino.Geometry;

namespace Ironbug.Grasshopper.Component.Ironbug
{
    public class Ironbug_SetpointManagerOutdoorAirReset : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_SetpointManagerOutdoorAirReset class.
        /// </summary>
        public Ironbug_SetpointManagerOutdoorAirReset()
          : base("Ironbug_SetpointManagerOutdoorAirReset", "Nickname",
              "Description",
              "Ironbug", "01:SetpointManager")
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
            pManager.AddGenericParameter("SetpointManagerOutdoorAirReset", "OAReset", "TODO:...", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_SetpointManagerOutdoorAirReset();

            //var settingParams = new Dictionary<IB_DataField, object>();
            //if (DA.GetData("Parameters", ref settingParams))
            //{
            //    obj.SetAttributes(settingParams);
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
            get { return new Guid("f251c255-bb89-4a16-8339-d7adbbdc474a"); }
        }
    }
}