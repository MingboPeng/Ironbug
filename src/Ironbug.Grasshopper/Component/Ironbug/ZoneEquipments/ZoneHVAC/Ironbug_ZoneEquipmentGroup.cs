using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Ironbug.HVAC.BaseClass;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ZoneEquipmentGroup : Ironbug_Component
    {
        
        /// Initializes a new instance of the Ironbug_ZoneEquipmentGroup class.
        
        public Ironbug_ZoneEquipmentGroup()
          : base("IB_ZoneEquipmentGroup", "ZoneEquipmentGroup",
              "Use Ironbug_ZoneEquipmentGroup if you have more than one equipments for one zone",
               "Ironbug", "04:ZoneEquipments")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("ZoneEquipments", "Equips_", "A list of zone equipments that will be grouped.", GH_ParamAccess.list);
            pManager[0].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ZoneEquipmentGroup", "EquipGroup", "A list of zone equipment groups that will be added to each zones.Connect to Ironbug_ThermalZone", GH_ParamAccess.item);
            pManager[0].DataMapping = GH_DataMapping.Flatten;
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var zoneEqps = new List<IB_ZoneEquipment>();
            DA.GetDataList(0, zoneEqps);

            var group = new HVAC.IB_ZoneEquipmentGroup(zoneEqps);
            DA.SetData(0, group);

        }
        protected override System.Drawing.Bitmap Icon => Properties.Resources.zoneEqpGroup;

        public override Guid ComponentGuid => new Guid("73fb35a3-4bc5-441a-b922-f0a90f0b6d5b");
    }


}