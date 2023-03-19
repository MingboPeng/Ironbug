using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_PlantEquipmentOperationCoolingLoad : Ironbug_Component
    {
       
        public Ironbug_PlantEquipmentOperationCoolingLoad()
          : base("IB_PlantEquipmentOperationCoolingLoad", "ClnLoadScheme",
              "Description",
              "Ironbug", "06:Sizing & Controller")
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary | GH_Exposure.obscure;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("UpperLimits", "limits_", "UpperLimits. 1,000,000,000 (W) will be assigned by default", GH_ParamAccess.list);
            pManager[0].Optional = true;
            pManager.AddGenericParameter("Equipments", "_equips", "Equipments", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CoolingLoadScheme", "scheme", "CoolingLoadScheme", GH_ParamAccess.item);
        }
        
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_PlantEquipmentOperationCoolingLoad();
            var limits = new List<int>();
            var eqps = new List<HVAC.BaseClass.IB_HVACObject>();

            DA.GetDataList(0, limits);
            DA.GetDataList(1, eqps);

            var c = eqps?.Count - limits?.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    limits.Add(1000000000);
                }
            }

            for (int i = 0; i < eqps.Count; i++)
            {
                obj.AddEquipment(limits[i], eqps[i]);
            }

            DA.SetData(0, obj);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.PlantCoolingScheme;
        
        public override Guid ComponentGuid => new Guid("86E1ED29-5606-4799-B48D-E34C93F7AAC0");
    }
}