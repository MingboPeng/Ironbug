using System;
using Grasshopper.Kernel;
using Ironbug.HVAC;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ZoneHVACEnergyRecoveryVentilator : Ironbug_HVACComponent
    {
        
        public Ironbug_ZoneHVACEnergyRecoveryVentilator()
          : base("Ironbug_ZoneHVACEnergyRecoveryVentilator", "ZoneERV",
              "Description",
              "Ironbug", "04:ZoneEquipments",
              typeof(IB_ZoneHVACEnergyRecoveryVentilator_DataFieldSet))
        {
        }
        protected override System.Drawing.Bitmap Icon => Properties.Resources.zoneERV;

        public override Guid ComponentGuid => new Guid("B117A73D-597E-4922-9E34-F93F081EBE2F");

        public override GH_Exposure Exposure => GH_Exposure.quinary;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("HeatingExchanger", "_HeatEx", "Must be HeatExchangerAirToAirSensibleAndLatent.", GH_ParamAccess.item);
            pManager.AddGenericParameter("SupplyFan", "_spFan", "Fan:OnOff.", GH_ParamAccess.item);
            pManager.AddGenericParameter("ExhaustFan", "_exFan", "Fan:OnOff.", GH_ParamAccess.item);

            pManager[0].Optional = true;
            pManager[1].Optional = true;
            pManager[2].Optional = true;

        }
        
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ZoneHVACEnergyRecoveryVentilator", "ZoneERV", "Connect to zone's equipment", GH_ParamAccess.item);
        }
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            var heatingEx = new IB_HeatExchangerAirToAirSensibleAndLatent();
            var spFan = new IB_FanOnOff();
            var exFan = new IB_FanOnOff();

            DA.GetData(0, ref heatingEx);
            DA.GetData(1, ref spFan);
            DA.GetData(2, ref exFan);

            
            var obj = new HVAC.IB_ZoneHVACEnergyRecoveryVentilator(heatingEx, spFan, exFan);
            

            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }

        
    }
}