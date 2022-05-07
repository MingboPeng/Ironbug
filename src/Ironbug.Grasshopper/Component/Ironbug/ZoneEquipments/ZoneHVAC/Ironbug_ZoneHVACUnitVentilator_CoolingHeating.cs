﻿using System;
using Grasshopper.Kernel;
using Ironbug.HVAC;
using Ironbug.HVAC.BaseClass;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ZoneHVACUnitVentilator_CoolingHeating : Ironbug_HVACWithParamComponent
    {
        public Ironbug_ZoneHVACUnitVentilator_CoolingHeating()
          : base("IB_ZoneHVACUnitVentilator_CoolingHeating", "UnitVentCoolingHeating",
              "Description",
              "Ironbug", "04:ZoneEquipments",
              typeof(IB_ZoneHVACUnitVentilator_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("HeatingCoil", "coilH_", "Heating coil to provide reheat source. can be CoilHeatingWater, CoilHeatingElectirc, or CoilHeatingGas.", GH_ParamAccess.item);
            pManager[0].Optional = true;
            pManager.AddGenericParameter("CoolingCoil", "coilC_", "Cooling coil to provide cooling source. can be CoilHeatingWater, CoilHeatingElectirc, or CoilHeatingGas.", GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager.AddGenericParameter("Fan", "fan_", "Can be FanConstantVolume or FanVariableVolume.", GH_ParamAccess.item);
            pManager[2].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ZoneHVACUnitVentilator_CoolingHeating", "UnitVent", "Connect to zone's equipment", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_ZoneHVACUnitVentilator_CoolingHeating();
            

            var fan = (IB_Fan)null;
            var coilH = (IB_CoilHeatingBasic)null;
            var coilC = (IB_CoilCoolingBasic)null;

            if (DA.GetData(0, ref coilH))
            {
                obj.SetHeatingCoil(coilH);
            }

            if (DA.GetData(1, ref coilC))
            {
                obj.SetCoolingCoil(coilC);
            }

            if (DA.GetData(2, ref fan))
            {
                obj.SetFan(fan);
            }


            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.UnitVentHC;

        public override Guid ComponentGuid => new Guid("25BF6732-9096-4D90-8587-3C0F61257349");
    }
}