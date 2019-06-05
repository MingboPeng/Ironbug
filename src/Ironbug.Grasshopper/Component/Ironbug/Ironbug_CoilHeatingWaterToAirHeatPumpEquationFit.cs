using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CoilHeatingWaterToAirHeatPumpEquationFit : Ironbug_HVACComponent
    {
        
        /// Initializes a new instance of the Ironbug_CoilHeatingWater class.
        
        public Ironbug_CoilHeatingWaterToAirHeatPumpEquationFit()
          : base("Ironbug_CoilHeatingWaterToAirHeatPumpEquationFit", "CoilHtn_WaterToAir",
              "Description",
              "Ironbug", "04:ZoneEquipments",
              typeof(HVAC.IB_CoilHeatingWaterToAirHeatPumpEquationFit_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.quinary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CoilHeatingWaterToAirHeatPumpEquationFit", "Coil", "Connect to ZoneHVACWaterToAirHeatPump", GH_ParamAccess.item);
            pManager.AddGenericParameter("WaterSide", "ToWaterLoop", "Connect to hot water loop's demand side via plantBranches", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_CoilHeatingWaterToAirHeatPumpEquationFit();
            
            var objs = this.SetObjParamsTo(obj);
            DA.SetDataList(0, objs);
            DA.SetDataList(1, objs);
        }


        protected override System.Drawing.Bitmap Icon => Properties.Resources.Coil_HeatingWAFit;


        public override Guid ComponentGuid => new Guid("8DDCC5FD-87AB-481C-ADC2-607840D29AC6");

    }
    
}