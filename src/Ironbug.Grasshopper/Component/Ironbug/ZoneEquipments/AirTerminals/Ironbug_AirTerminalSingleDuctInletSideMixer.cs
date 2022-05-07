using System;

using Grasshopper.Kernel;
using Ironbug.HVAC;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_AirTerminalSingleDuctInletSideMixer : Ironbug_HVACWithParamComponent
    {
        public Ironbug_AirTerminalSingleDuctInletSideMixer()
          : base("IB_AirTerminalSingleDuctInletSideMixer", "InletMixer",
              "Description",
              "Ironbug", "03:AirTerminals",
              typeof(IB_AirTerminalSingleDuctInletSideMixer_FieldSet))
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("ZoneEquipment", "zoneEqp_", "ZoneEquipment that mixes its air with this air terminal supply air at its inlet side.\nThis ZoneEquipment has to be also plugged in ThermalZone's ZoneEquipment list input.", GH_ParamAccess.item);
   
        }

        
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("AirTerminalSingleDuctInletSideMixer", "AT", "connect to Zone", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var eqp = (HVAC.BaseClass.IB_ZoneEquipment)null;
            
            if (DA.GetData(0, ref eqp))
            {
                var obj = new IB_AirTerminalSingleDuctInletSideMixer(eqp);
                this.SetObjParamsTo(obj);
                DA.SetData(0, obj);
            }


        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.AirTerminalMixer;

        public override Guid ComponentGuid => new Guid("{6228FAE5-C1DB-470E-AD7A-C8E4D4FB431C}");
    }
}