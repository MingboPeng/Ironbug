using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ChillerElectricEIR_Air : Ironbug_HVACComponent
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_ChillerElectricEIR class.
        /// </summary>
        public Ironbug_ChillerElectricEIR_Air()
          : base("Ironbug_ChillerElectricEIR_AirCooled", "Chiller_AirCooled",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_ChillerElectricEIR_FieldSet))
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
            pManager.AddGenericParameter("ChillerElectricEIR_AirCooled", "Chiller", "Connect to chilled water loop's supply side.", GH_ParamAccess.item);
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
        
        protected override System.Drawing.Bitmap Icon => Properties.Resources.Chiller_Air;
        
        public override Guid ComponentGuid => new Guid("BDE02476-2348-4258-9291-FAA0F48A16C0");
    }
}