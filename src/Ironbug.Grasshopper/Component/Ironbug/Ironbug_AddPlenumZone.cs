using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Ironbug.Grasshopper.Properties;
using Ironbug.HVAC.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_AddPlenumZone : Ironbug_Component
    {
      
        protected override System.Drawing.Bitmap Icon => Resources.AddPlenum;

        public override Guid ComponentGuid => new Guid("B26C1AD6-D590-4C79-8E93-F410F0C1CF25");

        public override GH_Exposure Exposure => GH_Exposure.primary;

        
        public Ironbug_AddPlenumZone()
          : base("IB_PlenumZone", "PlenumZone",
              "Attach supply or/and retrun plenum zones to thermal zones in the airloop.",
              "Ironbug", "00:Ironbug")
        {
        }
        
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("HoneybeePlenumZone", "supplyPlenum_", "Use HBRoom or OsZones.\nThis plenumZone will be connect to all thermal zones", GH_ParamAccess.list);
            pManager[0].Optional = true;
            pManager.AddGenericParameter("IB_ThermalZones", "_ThermalZones", "Add Ironbug_ThermalZones to here to attach their supply and return plenums.", GH_ParamAccess.list);
            pManager.AddGenericParameter("HoneybeePlenumZone", "returnPlenum_", "Use HBRoom or OsZones.\nThis plenumZone will be connect to all thermal zones", GH_ParamAccess.list);
            pManager[2].Optional = true;
     
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("OpenStudio ThermalZone", "Zones", "ThermalZones with plenumZones attached,\nconnect to airloop's demand side", GH_ParamAccess.list);
        }
        

        protected override void SolveInstance(IGH_DataAccess DA)
        {

            var HBPlenumZones_supply = new List<object>();
            DA.GetDataList(0, HBPlenumZones_supply);
            if (HBPlenumZones_supply.Count > 1) AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Not sure how to add multiple supply plenumZones to all thermalZones");

            var HBPlenumZones_retrun = new List<object>();
            DA.GetDataList(2, HBPlenumZones_retrun);
            if (HBPlenumZones_retrun.Count > 1) AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Not sure how to add multiple supply plenumZones to all thermalZones");

            var thermalZones = new List<IB_ThermalZone>();
            if (!DA.GetDataList(1, thermalZones)) return;
            if (!thermalZones.Any()) return;

            var newZones = new List<IB_ThermalZone>();
            foreach (var item in thermalZones)
            {
                newZones.Add(item.Duplicate() as IB_ThermalZone);
            }

            IB_ThermalZone supplyPlenum = null;
            if (HBPlenumZones_supply.Any()) supplyPlenum = this.CreatePlenumZone(HBPlenumZones_supply[0]);
            if (supplyPlenum != null)
            {
                foreach (var item in newZones)
                {
                    item.SetSupplyPlenum(supplyPlenum);
                }
            }

            IB_ThermalZone returnPlenum = null;
            if (HBPlenumZones_retrun.Any()) returnPlenum = this.CreatePlenumZone(HBPlenumZones_retrun[0]);
            if (returnPlenum != null)
            {
                foreach (var item in newZones)
                {
                    item.SetReturnPlenum(returnPlenum);
                }
            }

            
            DA.SetDataList(0, newZones);
        }
        

        private IB_ThermalZone CreatePlenumZone(object HBZonesOrName)
        {
            var zoneName = Helper.GetRoomNames(new List<object> { HBZonesOrName })?.FirstOrDefault();

            if (string.IsNullOrEmpty(zoneName))
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "No valid HBZones or zone names!");
                return null;
            }

            var osZone = new IB_ThermalZone(zoneName);
            
            return osZone;
        }




    }
}