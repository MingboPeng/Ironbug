using System;

using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CoilCoolingDXMultiSpeed : Ironbug_HVACComponent
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_ChillerElectricEIR class.
        /// </summary>
        public Ironbug_CoilCoolingDXMultiSpeed()
          : base("Ironbug_CoilCoolingDXMultiSpeed", "CoilCDXMtp",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_CoilCoolingDXMultiSpeed_DataFieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;
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
            pManager.AddGenericParameter("CoilCoolingDXMultiSpeed", "CoilCDXMtp", "CoilCoolingDXMultiSpeed", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_CoilCoolingDXMultiSpeed();
            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
            this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "This obj is not fully finished by OpenStudio, stay tuned!");
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
                return Properties.Resources.CoilCDXM;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("E76307B8-27F4-4C86-AB23-0864160B725E"); }
        }
    }
}