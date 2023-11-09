using System;
using Grasshopper.Kernel;
using Ironbug.HVAC;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_AirLoopHVACUnitaryHeatPumpAirToAir : Ironbug_HVACWithParamComponent
    {
        public Ironbug_AirLoopHVACUnitaryHeatPumpAirToAir()
          : base("IB_AirLoopHVACUnitaryHeatPumpAirToAir", "HeatPumpAirToAir",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(IB_AirLoopHVACUnitaryHeatPumpAirToAir_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary | GH_Exposure.obscure;

        protected override System.Drawing.Bitmap Icon => Properties.Resources.UnitaryHP;

        public override Guid ComponentGuid => new Guid("13B7C0BF-735A-4B85-9C48-DC3571A84934");

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("HeatingCoil", "_coilH_", $"{EPDoc.AirLoopHVACUnitaryHeatPumpAirToAir.Field_HeatingCoilName} \n\n{EPDoc.AirLoopHVACUnitaryHeatPumpAirToAir.Field_HeatingCoilObjectType}", GH_ParamAccess.item);
            pManager[0].Optional = true;
            pManager.AddGenericParameter("CoolingCoil", "_coilC_", $"{EPDoc.AirLoopHVACUnitaryHeatPumpAirToAir.Field_CoolingCoilName} \n\n{EPDoc.AirLoopHVACUnitaryHeatPumpAirToAir.Field_CoolingCoilObjectType}", GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager.AddGenericParameter("Fan", "_fan_", $"{EPDoc.AirLoopHVACUnitaryHeatPumpAirToAir.Field_SupplyAirFanName} \n\n{EPDoc.AirLoopHVACUnitaryHeatPumpAirToAir.Field_SupplyAirFanObjectType}", GH_ParamAccess.item);
            pManager[2].Optional = true;
            pManager.AddGenericParameter("SupplementalHeatingCoil", "spCoilH_", $"{EPDoc.AirLoopHVACUnitaryHeatPumpAirToAir.Field_SupplementalHeatingCoilName} \n\n{EPDoc.AirLoopHVACUnitaryHeatPumpAirToAir.Field_SupplementalHeatingCoilObjectType}", GH_ParamAccess.item); 
            pManager[3].Optional = true;

            pManager.AddGenericParameter("ControllingZone", "ctrlZone_", "The controlling zone for thermostat location. It is required to set a valid when the unitary system is used within an air loop.", GH_ParamAccess.item);
            pManager[4].Optional = true;

        }
        
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("UnitarySystem", "UniSys", "Connect to airloop's supply side", GH_ParamAccess.item);
        }


        protected override void SolveInstance(IGH_DataAccess DA)
        {
            HVAC.BaseClass.IB_Fan fan = null;
            HVAC.BaseClass.IB_CoilDX coilH = null;
            HVAC.BaseClass.IB_CoilDX coilC = null;
            HVAC.BaseClass.IB_CoilHeatingBasic spCoilH = null;
            HVAC.BaseClass.IB_ThermalZone zone = null;

            var obj = new HVAC.IB_AirLoopHVACUnitaryHeatPumpAirToAir();
            if (DA.GetData(0, ref coilH)) obj.SetHeatingCoil(coilH);
            if (DA.GetData(1, ref coilC)) obj.SetCoolingCoil(coilC);
            if (DA.GetData(2, ref fan)) obj.SetFan(fan);
            if (DA.GetData(3, ref spCoilH)) obj.SetSupplementalHeatingCoil(spCoilH);

            if (DA.GetData(4, ref zone)) obj.SetControllingZone(zone);
            
            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }
        
    }
}