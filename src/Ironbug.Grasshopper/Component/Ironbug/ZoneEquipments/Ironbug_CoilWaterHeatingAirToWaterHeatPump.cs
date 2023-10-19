﻿using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CoilWaterHeatingAirToWaterHeatPump : Ironbug_DuplicableHVACWithParamComponent
    {
        public Ironbug_CoilWaterHeatingAirToWaterHeatPump()
          : base("IB_CoilWaterHeatingAirToWaterHeatPump", "CoilWHHP",
              "Description",
              "Ironbug", "04:ZoneEquipments",
              typeof(HVAC.IB_CoilWaterHeatingAirToWaterHeatPump_FieldSet))
        {
        }
        public override GH_Exposure Exposure => GH_Exposure.quinary;
        
        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Coil", "Coil", "This is a coil used in WaterHeaterHeatPump", GH_ParamAccess.item);
        }

        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_CoilWaterHeatingAirToWaterHeatPump();
            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }


        protected override System.Drawing.Bitmap Icon => Properties.Resources.Coil_WHHP;

        public override Guid ComponentGuid => new Guid("B5CCA2DB-5E30-450D-83E8-A2C5C9DAF5CE");
    }
}