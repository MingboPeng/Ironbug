using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC
{
    public class IB_PlantLoop:IB_ModelObject, IIB_ToOPSable
    {
        private IList<IIB_PlantLoopObjects> supplyComponents { get; set; } = new List<IIB_PlantLoopObjects>();
        private IList<IIB_DualLoopObjects> demandComponents { get; set; } = new List<IIB_DualLoopObjects>();

        private static PlantLoop InitMethod(Model model) => new PlantLoop(model);
        public IB_PlantLoop():base(InitMethod(new Model()))
        {
        }

        public void AddToSupplyBranch(IIB_PlantLoopObjects HvacComponent)
        {
            this.supplyComponents.Add(HvacComponent);
        }

        public void AddToDemandBranch(IIB_DualLoopObjects HvacComponent)
        {
            this.demandComponents.Add(HvacComponent);
        }
        
        
        public ModelObject ToOS(Model model)
        {
            var plant = base.ToOS(InitMethod, model).to_PlantLoop().get();

            //TODO: add IB_Loop to take care of branches matter
            // below this is for temporary testing purpose before supply branch is finished.
            //var boiler = new BoilerHotWater(model);
            //plant.addSupplyBranchForComponent(boiler);

            foreach (var item in supplyComponents)
            {
                plant.addSupplyBranchForComponent((HVACComponent)item.ToOS(model));
            }

            foreach (var item in demandComponents)
            {
                plant.addDemandBranchForComponent((HVACComponent)item.ToOS(model));
            }

            
            return plant;
        }

        public override IB_ModelObject Duplicate()
        {
            //TODO: duplicate child objects
            return this.DuplicateIB_ModelObject(() => new IB_PlantLoop());
        }
    }
}
