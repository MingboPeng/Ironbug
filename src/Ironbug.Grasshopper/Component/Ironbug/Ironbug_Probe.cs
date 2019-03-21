using System;
using Grasshopper.Kernel;
using Ironbug.Grasshopper.Properties;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_Probe : Ironbug_HVACComponent
    {
        
        /// <summary>
        /// Initializes a new instance of the Ironbug_FanConstantVolume class.
        /// </summary>
        public Ironbug_Probe()
          : base("Ironbug_Probe", "Probe",
              "Use this component to measure variables like temperature, flow rate, etc, in the loop.\nPlace this between loopObjects.",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_Probe_FieldSet))
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
            pManager.AddGenericParameter("Probe", "Probe", "TODO....", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_Probe();

            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon => null;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("1590EA51-3858-4974-AE8F-AAA31411F7CE");


    }

   
}