using System;
using Grasshopper.Kernel;
using Ironbug.Grasshopper.Properties;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_PipeOutdoor : Ironbug_HVACComponent
    {
        
        /// <summary>
        /// Initializes a new instance of the Ironbug_FanConstantVolume class.
        /// </summary>
        public Ironbug_PipeOutdoor()
          : base("Ironbug_PipeOutdoor", "PipeOutdoor",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_PipeOutdoor_FieldSet))
        {
            
        }

        public override GH_Exposure Exposure => GH_Exposure.septenary;

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
            pManager.AddGenericParameter("PipeOutdoor", "Pipe", "PipeOutdoor", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_PipeOutdoor();

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
                return Resources.PipeOutdoor;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("2E05B317-AC4A-458E-9089-A6BDB88FF245"); }
        }

        
    }

   
}