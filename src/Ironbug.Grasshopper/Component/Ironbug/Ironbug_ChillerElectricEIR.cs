using System;

using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ChillerElectricEIR : Ironbug_HVACComponentBase
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_ChillerElectricEIR class.
        /// </summary>
        public Ironbug_ChillerElectricEIR()
          : base("Ironbug_ChillerElectricEIR", "ChillerE",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_ChillerElectricEIR_DataFieldSet))
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
            pManager.AddGenericParameter("ChillerElectricEIR", "ChillerE", "Connect to chilled water loop's supply side.", GH_ParamAccess.item);
            pManager.AddGenericParameter("ChillerElectricEIR_CondenserLoop", "ToCondenser", "Connect to condenser loop's demand side.", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_ChillerElectricEIR();
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
                return Properties.Resources.Chiller;

            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("9cba9235-6cc3-498e-b653-6eda9870d276"); }
        }
    }
}