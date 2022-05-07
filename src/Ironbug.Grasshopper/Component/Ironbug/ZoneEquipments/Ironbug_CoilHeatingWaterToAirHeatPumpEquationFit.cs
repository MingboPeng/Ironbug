﻿using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CoilHeatingWaterToAirHeatPumpEquationFit : Ironbug_DuplicableHVACWithParamComponent
    {
        
        /// Initializes a new instance of the Ironbug_CoilHeatingWater class.
        
        public Ironbug_CoilHeatingWaterToAirHeatPumpEquationFit()
          : base("IB_CoilHeatingWaterToAirHeatPumpEquationFit", "CoilHtn_WaterToAir",
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
           pManager[ pManager.AddGenericParameter("WaterSide", "ToWaterLoop", "Connect to hot water loop's demand side via plantBranches", GH_ParamAccess.item)].DataMapping = GH_DataMapping.Graft;
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_CoilHeatingWaterToAirHeatPumpEquationFit();

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
            DA.SetDataList(1, objs);
        }


        protected override System.Drawing.Bitmap Icon => Properties.Resources.Coil_HeatingWAFit;


        public override Guid ComponentGuid => new Guid("8DDCC5FD-87AB-481C-ADC2-607840D29AC6");

    }
    
}