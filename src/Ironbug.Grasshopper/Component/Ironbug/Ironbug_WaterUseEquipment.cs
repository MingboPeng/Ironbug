using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_WaterUseEquipment : Ironbug_HVACWithParamComponent
    {
        
        /// Initializes a new instance of the Ironbug_ZoneEquipmentGroup class.
        
        public Ironbug_WaterUseEquipment()
          : base("IB_WaterUseEquipment", "WaterUseEquipment",
              "Description",
               "Ironbug", "02:LoopComponents",
               typeof(HVAC.IB_WaterUseEquipment_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("WaterUseLoad", "load_", "Use Ironbug_WaterUseEquipmentDefinition", GH_ParamAccess.item);
            pManager[0].Optional = true;
            pManager.AddGenericParameter("HBRoom", "HBRoom_", "Honeybee Room", GH_ParamAccess.item);
            pManager[1].Optional = true;
        }
        
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("WaterUseEquipment", "waterEqp", "Connect to Ironbug_WaterUseConnections", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            HVAC.IB_WaterUseEquipmentDefinition load = new HVAC.IB_WaterUseEquipmentDefinition();
            DA.GetData(0, ref load);

            object HBRoom = null;
            var spaceName = string.Empty;
            DA.GetData(1, ref HBRoom);
            if (HBRoom != null)
                spaceName = GetRoomName(HBRoom);

            var obj = new HVAC.IB_WaterUseEquipment(load);
            obj.SetSpace(spaceName);

            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);

        }
        private string GetRoomName(object HBZonesOrName)
        {
            var zoneName = string.Empty;

            if (HBZonesOrName is GH_Brep hbzones)
            {
                zoneName = Helper.CallFromHBHive(new List<GH_Brep>() { hbzones }).FirstOrDefault();
            }
            else if (HBZonesOrName is GH_String nameString)
            {
                zoneName = nameString.Value;
            }
            else if (HBZonesOrName is GH_ObjectWrapper wrapper)
            {
                // LBT Room
                var isLBTRoom = wrapper.Value.ToString().StartsWith("Room:");
                isLBTRoom &= wrapper.Value.GetType().ToString().StartsWith("IronPython.");

                if (isLBTRoom)
                {
                    zoneName = Helper.FromLBTRooms(new List<object> { HBZonesOrName }).FirstOrDefault();
                }

            }
            return zoneName;
        }
        protected override System.Drawing.Bitmap Icon => Properties.Resources.WaterUseEquip;

        public override Guid ComponentGuid => new Guid("3A46B628-A815-4994-A919-5FD10CB87CF1");


    }



}