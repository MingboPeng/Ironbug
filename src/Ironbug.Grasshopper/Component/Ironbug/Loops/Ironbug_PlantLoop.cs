using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Ironbug.Grasshopper.Properties;
using Ironbug.HVAC.BaseClass;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_PlantLoop : Ironbug_HVACWithParamComponent
    {
        public Ironbug_PlantLoop()
          : base("IB_PlantLoop", "PlantLoop",
              "Description",
              "Ironbug", "01:Loops",
              typeof(HVAC.IB_PlantLoop_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary | GH_Exposure.obscure;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("_supply", "_supply", "HVAC components", GH_ParamAccess.list);
            //pManager[0].Optional = true;
            pManager.AddGenericParameter("_demand", "_demand", "HVAC components", GH_ParamAccess.list);
            //pManager[1].Optional = true;
            pManager.AddGenericParameter("sizingLoop", "sizing", "HVAC components", GH_ParamAccess.item);
            pManager[2].Optional = true;
            pManager.AddGenericParameter("Operation Scheme", "scheme", "HVAC components", GH_ParamAccess.item);
            pManager[3].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("plantLoop", "plantLoop", "plantLoop", GH_ParamAccess.item);
        }

        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<IB_HVACObject> supplyComs = new List<IB_HVACObject>();
            List<IB_HVACObject> demandComs = new List<IB_HVACObject>();
            HVAC.IB_SizingPlant sizing = null;
            HVAC.IB_PlantEquipmentOperationSchemeBase scheme = null;
            DA.GetDataList(0, supplyComs);
            DA.GetDataList(1, demandComs);
            DA.GetData(2, ref sizing);
            DA.GetData(3, ref scheme);

            var plant = new HVAC.IB_PlantLoop();
            foreach (var item in supplyComs)
            {
                var newItem = (IB_HVACObject)item.Duplicate();
                plant.AddToSupply(newItem);
            }
            foreach (var item in demandComs)
            {
                var newItem = (IB_HVACObject)item.Duplicate();
                plant.AddToDemand(newItem);
            }

            if (sizing != null) 
                plant.SetSizingPlant(sizing);

            if (scheme != null)
                plant.SetOperationScheme(scheme);

            this.SetObjParamsTo(plant);
            DA.SetData(0, plant);

            this.Message = this.RunCount == 1 ? $"{this.RunCount} Loop" : $"{this.RunCount} Loops";
        }
        protected override System.Drawing.Bitmap Icon => Resources.PlantLoop;


        public override Guid ComponentGuid => new Guid("172A074C-8FFC-45FE-9E0D-4017591F3BAC");

        public override void CreateAttributes()
        {
            var newAttri = new IB_LoopComponentAttributes(this);
            m_attributes = newAttri;
        }
    }
}