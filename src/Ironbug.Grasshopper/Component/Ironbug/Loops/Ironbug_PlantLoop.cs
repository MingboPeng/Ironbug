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
          : base("Ironbug_PlantLoop", "PlantLoop",
              "Description",
              "Ironbug", "01:Loops",
              typeof(HVAC.IB_PlantLoop_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary | GH_Exposure.obscure;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("supply", "supply", "HVAC components", GH_ParamAccess.list);
            pManager[0].Optional = true;
            pManager.AddGenericParameter("demand", "demand", "HVAC components", GH_ParamAccess.list);
            pManager[1].Optional = true;
            pManager.AddGenericParameter("sizingLoop", "sizing", "HVAC components", GH_ParamAccess.item);
            pManager[2].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PlantLoop", "PlantLoop", "PlantLoop", GH_ParamAccess.item);
        }

        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<IB_HVACObject> supplyComs = new List<IB_HVACObject>();
            List<IB_HVACObject> demandComs = new List<IB_HVACObject>();
            HVAC.IB_SizingPlant sizing = null;
            DA.GetDataList(0, supplyComs);
            DA.GetDataList(1, demandComs);
            DA.GetData(2, ref sizing);


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
            {
                plant.SetSizingPlant(sizing);
            }


            this.SetObjParamsTo(plant);
            DA.SetData(0, plant);

            this.Message = this.RunCount == 1 ? $"{this.RunCount} Loop" : $"{this.RunCount} Loops";
        }
        protected override System.Drawing.Bitmap Icon => Resources.PlantLoop;


        public override Guid ComponentGuid => new Guid("63e4f976-4b63-48e4-b6f7-a2b5d7040252");

        public override void CreateAttributes()
        {
            var newAttri = new IB_LoopComponentAttributes(this);
            m_attributes = newAttri;
        }
    }
}