using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CoilHeatingWaterToAirHeatPumpEquationFit : Ironbug_HVACComponent
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_CoilHeatingWater class.
        /// </summary>
        public Ironbug_CoilHeatingWaterToAirHeatPumpEquationFit()
          : base("Ironbug_CoilHeatingWaterToAirHeatPumpEquationFit", "CoilHtn_WaterToAir",
              "Description",
              "Ironbug", "04:ZoneEquipments",
              typeof(HVAC.IB_CoilHeatingWaterToAirHeatPumpEquationFit_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.quinary;

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
            pManager.AddGenericParameter("CoilHeatingWaterToAirHeatPumpEquationFit", "Coil", "Connect to ZoneHVACWaterToAirHeatPump", GH_ParamAccess.item);
            pManager.AddGenericParameter("WaterSide", "ToWaterLoop", "Connect to hot water loop's demand side via plantBranches", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_CoilHeatingWaterToAirHeatPumpEquationFit();
            
            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
            DA.SetData(1, obj);
        }



        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Properties.Resources.Coil_HeatingWAFit;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("8DDCC5FD-87AB-481C-ADC2-607840D29AC6"); }
        }
        
    }
    
}