using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC
{
    public class IB_PlantLoop:IB_ModelObject
    {
        private List<IB_HVACComponent> demandComponents { get; set; } = new List<IB_HVACComponent>();
        private static PlantLoop InitMethod(Model model) => new PlantLoop(model);
        public IB_PlantLoop():base(InitMethod(new Model()))
        {
            base.SetName("PlantLoop");
        }

        public void AddToDemandBranch(IB_HVACComponent HvacComponent)
        {
            this.demandComponents.Add(HvacComponent);
        }
        
        
        public override ParentObject ToOS(Model model)
        {
            var plant = base.ToOS(InitMethod, model).to_PlantLoop().get();

            //TODO: add IB_Loop to take care of branches matter
            // below this is for temporary testing purpose before supply branch is finished.
            var boiler = new BoilerHotWater(model);
            plant.addSupplyBranchForComponent(boiler);


            foreach (var item in demandComponents)
            {
                plant.addDemandBranchForComponent((HVACComponent)item.ToOS(model));
            }


            return plant;
        }

        public override IB_ModelObject Duplicate()
        {
            return this.Duplicate(() => new IB_PlantLoop());
        }
    }
}
