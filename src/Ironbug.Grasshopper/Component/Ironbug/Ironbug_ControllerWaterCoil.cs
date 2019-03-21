using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ControllerWaterCoil : Ironbug_HVACComponent
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_OAController class.
        /// </summary>
        public Ironbug_ControllerWaterCoil()
          : base("Ironbug_ControllerWaterCoil", "ControllerWaterCoil",
              "Description",
              "Ironbug", "06:Sizing&Controller",
              typeof(HVAC.IB_ControllerWaterCoil_FieldSet))
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
            pManager.AddGenericParameter("ControllerWaterCoil", "Ctrl", "connect to advanced CoilCoolingWater or CoilHeatingWater", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_ControllerWaterCoil();
            
            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);

        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Properties.Resources.WaterCoilCtrl;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("ABF078CD-62C6-4820-8B5B-DC715041A32A");
    }
}