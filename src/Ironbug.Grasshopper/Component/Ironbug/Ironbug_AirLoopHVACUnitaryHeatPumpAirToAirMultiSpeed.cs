using System;
using Grasshopper.Kernel;
using Ironbug.HVAC;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_AirLoopHVACUnitaryHeatPumpAirToAirMultiSpeed : Ironbug_HVACWithParamComponent
    {
        public Ironbug_AirLoopHVACUnitaryHeatPumpAirToAirMultiSpeed()
          : base("IB_AirLoopHVACUnitaryHeatPumpAirToAirMultiSpeed", "HeatPumpAirToAirM",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(IB_AirLoopHVACUnitaryHeatPumpAirToAirMultiSpeed_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary | GH_Exposure.obscure;

        protected override System.Drawing.Bitmap Icon => Properties.Resources.UnitaryHP;

        public override Guid ComponentGuid => new Guid("{CB57036D-231E-4B05-A21A-CED6106F6ABE}");

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("HeatingCoil", "_coilH_", $"{EPDoc.AirLoopHVACUnitaryHeatPumpAirToAirMultiSpeed.Field_HeatingCoilName} \n\n{EPDoc.AirLoopHVACUnitaryHeatPumpAirToAirMultiSpeed.Field_HeatingCoilObjectType}", GH_ParamAccess.item);
            pManager[0].Optional = true;
            pManager.AddGenericParameter("CoolingCoil", "_coilC_", $"{EPDoc.AirLoopHVACUnitaryHeatPumpAirToAirMultiSpeed.Field_CoolingCoilName} \n\n{EPDoc.AirLoopHVACUnitaryHeatPumpAirToAirMultiSpeed.Field_CoolingCoilObjectType}", GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager.AddGenericParameter("Fan", "_fan_", $"{EPDoc.AirLoopHVACUnitaryHeatPumpAirToAirMultiSpeed.Field_SupplyAirFanName} \n\n{EPDoc.AirLoopHVACUnitaryHeatPumpAirToAirMultiSpeed.Field_SupplyAirFanObjectType}", GH_ParamAccess.item);
            pManager[2].Optional = true;
            pManager.AddGenericParameter("SupplementalHeatingCoil", "spCoilH_", $"{EPDoc.AirLoopHVACUnitaryHeatPumpAirToAirMultiSpeed.Field_SupplementalHeatingCoilName} \n\n{EPDoc.AirLoopHVACUnitaryHeatPumpAirToAirMultiSpeed.Field_SupplementalHeatingCoilObjectType}", GH_ParamAccess.item); 
            pManager[3].Optional = true;

            pManager.AddGenericParameter("ControllingZone", "_ctrlZone_", "The controlling zone for thermostat location. It is required to set a valid when the unitary system is used within an air loop.", GH_ParamAccess.item);
            pManager[4].Optional = true;
        }
        
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("UnitarySystem", "UniSys", "Connect to airloop's supply side", GH_ParamAccess.item);
        }


        protected override void SolveInstance(IGH_DataAccess DA)
        {
            HVAC.BaseClass.IB_Fan fan = null;
            HVAC.BaseClass.IB_Coil coilH = null;
            HVAC.BaseClass.IB_Coil coilC = null;
            HVAC.BaseClass.IB_CoilHeatingBasic spCoilH = null;
            HVAC.BaseClass.IB_ThermalZone zone = null;

            var obj = new HVAC.IB_AirLoopHVACUnitaryHeatPumpAirToAirMultiSpeed();
            if (DA.GetData(0, ref coilH)) obj.SetHeatingCoil(coilH);
            if (DA.GetData(1, ref coilC)) obj.SetCoolingCoil(coilC);
            if (DA.GetData(2, ref fan)) obj.SetFan(fan);
            if (DA.GetData(3, ref spCoilH)) obj.SetSupplementalHeatingCoil(spCoilH);

            if (DA.GetData(4, ref zone)) obj.SetControlZone(zone.GetRoomName());

            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }
        
    }
}