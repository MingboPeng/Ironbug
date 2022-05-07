﻿using System;
using Grasshopper.Kernel;
using Ironbug.HVAC;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ZoneHVACPackagedTerminalAirConditioner : Ironbug_HVACWithParamComponent
    {
        public Ironbug_ZoneHVACPackagedTerminalAirConditioner()
          : base("IB_ZoneHVACPackagedTerminalAirConditioner", "PT AirConditioner",
              "Description",
              "Ironbug", "04:ZoneEquipments",
              typeof(IB_ZoneHVACPackagedTerminalAirConditioner_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

        protected override System.Drawing.Bitmap Icon => Properties.Resources.PTAC;

        public override Guid ComponentGuid => new Guid("104B43E5-E89F-4241-8180-9213B17086B5");

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("HeatingCoil", "_coilH", "Heating coil to provide reheat source. CoilHeatingWater", GH_ParamAccess.item);
            //pManager[0].Optional = true;
            pManager.AddGenericParameter("CoolingCoil", "coilC_", "Cooling coil to provide cooling source. CoilCoolingDX", GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager.AddGenericParameter("Fan", "fan_", "Can be FanConstantVolume or FanVariableVolume.", GH_ParamAccess.item);
            pManager[2].Optional = true;
        }
        
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ZoneHVACPackagedTerminalAirConditioner", "PTAC", "Connect to zone's equipment", GH_ParamAccess.item);
        }
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            
            var fan = new IB_FanConstantVolume();
            var coilH =  (IB_CoilHeatingWater) null;
            var coilC = new IB_CoilCoolingDXSingleSpeed();

            DA.GetData(0, ref coilH);
            DA.GetData(1, ref coilC);
            DA.GetData(2, ref fan);

            var obj = new HVAC.IB_ZoneHVACPackagedTerminalAirConditioner(fan,coilH,coilC);
            

            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }
        
    }
}