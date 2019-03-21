using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CoilHeatingElectric : Ironbug_HVACComponent
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_CoilHeatingElectric class.
        /// </summary>
        public Ironbug_CoilHeatingElectric()
          : base("Ironbug_CoilHeatingElectric", "CoilHtnElec",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_CoilHeatingWater_FieldSet))
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
            pManager.AddGenericParameter("CoilHeatingElectric", "Coil", "TODO:...", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_CoilHeatingElectric();
            

            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }
        
        protected override System.Drawing.Bitmap Icon => Properties.Resources.CoilHE;
        
        public override Guid ComponentGuid => new Guid("dc8ed4d8-4435-4134-b1e8-06362bb6d411");
    }
}