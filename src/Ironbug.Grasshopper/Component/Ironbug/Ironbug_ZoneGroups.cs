using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ZoneGroups : Ironbug_Component
    {
        public Ironbug_ZoneGroups()
          : base("IB_ZoneGroups", "ZoneGroups",
              "Use this component to get grouped zone names from Honeybee rooms, OsZones, or zone names",
              "Ironbug", "00:Ironbug")
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary | GH_Exposure.obscure;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("_HBRooms", "_HBRooms", "HBRoom, OsZones, or zone names", GH_ParamAccess.list);
        }


        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Zones", "Zones", "Identical zone names from HBRooms", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var HBZones = new List<object>();
            if (!DA.GetDataList(0, HBZones)) return;
            if (!HBZones.Any()) return;

            var zoneNames = Helper.GetRoomNames(HBZones);
            // check if there are rooms in the same zone
            zoneNames = zoneNames.Distinct().ToList();

            if (!zoneNames.Any())
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "No valid HBZones, or OsZones or text zone names!");
                //return;
            }

            DA.SetDataList(0, zoneNames);
        }

        
        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("9B69A632-040D-4EF5-BD6F-86EFE4167EF3");

    }

   
}