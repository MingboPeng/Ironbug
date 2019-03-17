using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component.Ironbug
{
    public class Ironbug_SetpointManagerScheduled : Ironbug_Component
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_SetpointManagerWarmest class.
        /// </summary>
        public Ironbug_SetpointManagerScheduled()
          : base("Ironbug_SetpointManagerScheduled", "SPM_Scheduled",
              EPDoc.SetpointManagerScheduled.Note,
              "Ironbug", "05:SetpointManager")
        {
        }
        public override GH_Exposure Exposure => GH_Exposure.primary;


        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Temperature", "T", "SetpointTemperature", GH_ParamAccess.item);
            
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SetpointManagerScheduled", "SPM", "TODO:...", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            double temperature = 12.7778;

            DA.GetData(0, ref temperature);

            var obj = new HVAC.IB_SetpointManagerScheduled(temperature);
            
            
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
                return Properties.Resources.SetPointScheduled;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("A2FE343D-A2BA-42C3-B54E-2CBEFDE7DDA1"); }
        }
    }
}