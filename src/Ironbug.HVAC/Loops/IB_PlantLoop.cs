using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC
{
    public class IB_PlantLoop : IB_Loop
    {
        private List<IB_HVACObject> supplyComponents { get; set; } = new List<IB_HVACObject>();
        private List<IB_HVACObject> demandComponents { get; set; } = new List<IB_HVACObject>();

        private static PlantLoop InitMethod(Model model) => new PlantLoop(model);
        public IB_PlantLoop() : base(InitMethod(new Model()))
        {
        }

        public void AddToSupply(IB_HVACObject HvacComponent)
        {
            this.supplyComponents.Insert(0, HvacComponent);
            //this.supplyComponents.Add(HvacComponent);
        }

        public void AddToDemand(IB_HVACObject HvacComponent)
        {
            this.demandComponents.Add(HvacComponent);
        }


        public override ModelObject ToOS(Model model)
        {
            var plant = base.ToOS(InitMethod, model).to_PlantLoop().get();



            this.AddSupplyObjects(plant, this.supplyComponents);

            //TDDO: addDemandObjects
            foreach (var item in demandComponents)
            {
                plant.addDemandBranchForComponent((HVACComponent)item.ToOS(model));
            }


            return plant;
        }

        private bool AddSupplyObjects(PlantLoop plant, List<IB_HVACObject> Components)
        {

            //Find the branch object first, and mark it. 
            //Reverce the objects order before the mark (supplyInletNode)
            //keep the order (supplyOutletNode);

            var objsBeforeBranch = base.GetObjsBeforeBranch(Components);
            var supplyBranchObj = (IB_PlantLoopBranches)Components.Find(_ => _ is IB_PlantLoopBranches);
            var objsAfterBranch = base.GetObjsAfterBranch(Components);


            //
            var spInletNode = plant.supplyInletNode();
            var comps = objsBeforeBranch.Where(_ => !(_ is IB_SetpointManager)).Where(_ => !(_ is IB_PlantLoopBranches));
            comps.ToList().ForEach(_ => _.AddToNode(spInletNode));

            supplyBranchObj.ToOS_Supply(plant);

            var spOutLetNode = plant.supplyOutletNode();
            comps = objsAfterBranch.Where(_ => !(_ is IB_SetpointManager)).Where(_ => !(_ is IB_PlantLoopBranches));
            comps.ToList().ForEach(_ => _.AddToNode(spOutLetNode));

            //TODO: add setpoint
            var addedObjs = plant.supplyComponents().Where(_ => _.comment().Contains("TrackingID"));
            var allcopied = addedObjs.Count() == Components.CountIncludesBranches();


            //TODO: might need to double check the setpoint order.
            allcopied &= this.AddSetPoints(plant, Components);

            if (!allcopied)
            {
                throw new Exception("Failed to add airloop demand components!");
            }

            return allcopied;
        }

        public override IB_ModelObject Duplicate()
        {
            //TODO: duplicate child objects
            return this.DuplicateIB_ModelObject(() => new IB_PlantLoop());
        }



    }

    public static class Extensions
    {
        public static int CountIncludesBranches(this IEnumerable<IB_HVACObject> enumerable)
        {
            var count = 0;
            foreach (var item in enumerable)
            {
                if (item is IB_LoopBranches)
                {
                    count += ((IB_LoopBranches)item).Count();
                }
                else
                {
                    count++;
                }
            }

            return count;
        }
    }
}

